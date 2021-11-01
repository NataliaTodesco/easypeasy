/* 
clase de abstraccion construida sobre la api de here los metodos que deberian usarse desde afuera son :
    constructor: 
    let mapa = new Mapa(elementoHTML) <= crea una instancia del objeto mapa
    inicializador:
    mapa.initialize() <= inicializa el objeto y lo bindea al elemento que se le paso al constructor 
                         aunque es posible usar algunas caracteristicas de la clase sin inicializarla no 
                         es para nada recomendable
    markPlace:
    mapa.markPlace('lugar') <= recibe un string con una direccion y marca en el mapa la mejor coincidencia
                             util para mostrar cada destino de envio al armar las hojas de ruta o en las 
                             listas de envo
    traceRoute:
    mapa.traceRoute(['lugar','lugar',...]) <= recibe un arreglo de string con direcciones y crea un trazo 
                                              de la ruta optima, seria lo que mostraria la hoja de ruta una 
                                              vez se agregaron todos los envios, retorna ademas la distancia en metros
    ej: (async () => {
            let dist = await map.traceRoute(['patio olmos', 'rondeu 225', 'orfeo superdomo'])
            console.log(dist); //25205 (esto es el recorrido ida y vuelta hasta la UTN que por ahora ocupa el lugar de deposito)
        })()

    Dependencias: para que esto funcione el html debe incluir los siguientes scripts events y ui son opcionales, 
    pero su omision generara fallos graficos

    <script src="https://js.api.here.com/v3/3.1/mapsjs-core.js" type="text/javascript" charset="utf-8"></script>
    <script src="https://js.api.here.com/v3/3.1/mapsjs-service.js" type="text/javascript" charset="utf-8"></script>
    <script src="https://js.api.here.com/v3/3.1/mapsjs-mapevents.js" type="text/javascript" charset="utf-8"></script>
    <script src="https://js.api.here.com/v3/3.1/mapsjs-ui.js" type="text/javascript" charset="utf-8"></script>
    <link rel="stylesheet" type="text/css" href="https://js.api.here.com/v3/3.1/mapsjs-ui.css" />

*/
class Mapa{
    
     apikey = 'IK9R4ao8QIWUZSEiiMZ8yPQIb4KBMLFwtlXwvIvsNqY'
     platform = new H.service.Platform({apikey: this.apikey})
     maptypes = this.platform.createDefaultLayers()
     polylines = new H.map.Group()
     markers = new H.map.Group()
     depot = { lng: -64.19324, lat: -31.44233 }
     //constructor de clase, recibe un html al cual se anclara en la interfaz
     constructor(elem){this.elem=elem}
     //inicializa el mapa en el elemento que se le haya pasado al coonstructor de la clase
     initialize() {
         this.map = new H.Map(
             this.elem,
             this.maptypes.vector.normal.map,
             {
                 zoom: 17,
                 center: { lng: -64.19324, lat: -31.44233 },
 
             })
         this.map.addObject(this.polylines)
         this.map.addObject(this.markers)
         var ui = H.ui.UI.createDefault(this.map, this.maptypes, 'es-ES');
         ui.removeControl('mapsettings');
         ui.getControl('zoom').setAlignment('bottom-left');
         ui.getControl('scalebar').setAlignment('top-right');
         
         var events = new H.mapevents.MapEvents(this.map);
         
         var behavior = new H.mapevents.Behavior(events);

         this.map.getViewPort().setPadding(50, 50, 50, 50)

         window.addEventListener('resize', () => this.map.getViewPort().resize());

         return this.map
     }
     //centra el mapap en una posicion geografica
     setCenter(pos) {
         this.map.setCenter(pos)
     }
     //coloca un marcador en el mapa y lo centra en esa posicion dada una direccion en texto plano
     async markPlace(dir){
        this.clean()
        let pos = await this.getCoordinate(dir)
        var marker = new H.map.Marker({ lng: 0, lat: 0 })
        this.markers.addObject(marker)
        marker.setGeometry(pos)
        this.map.setCenter(pos)
     }
     //recibe un array de direcciones en texto plano, traza la ruta mas corta, marca cada parada con un marcador y centra el mapa en su viewbox 
     async traceRoute(dirArr){
         var posArr = []
         this.clean()
         for (let i = 0; i < dirArr.length; i++) {
            let pos = await this.getCoordinate(dirArr[i])
            var marker = new H.map.Marker({ lng: 0, lat: 0 })
            this.markers.addObject(marker)
            marker.setGeometry(pos)
            posArr.push(pos)  
         }
         
         //console.log('posarr', await posArr[0]);
         
         return this.getRoute(posArr)
     }
     //obtiene coordenadas a partir de una direccion en texto plano, filtra caracteres no alfanumericos. Para evitar confusiones esta acotado a la provincia de Cordoba
    async getCoordinate(dir) {
        let query = dir.replace(/[^A-Za-z0-9]/g, ' ').split(" ").filter(e => e != '').join("+");
        query += '+Córdoba+Argentina'
        let url = `https://geocode.search.hereapi.com/v1/geocode?q=${query}&apiKey=${this.apikey}`
        
        let pos =
        fetch(url).then(res => { if (!res.ok) throw Error(res.status); return res })
            .then(res => res.json()).then(data => {
                if (data.items != 0) {
                    
                    return data.items[0].position
                   
                }
            }).catch(this.onError)
        return await pos

    }
    //pide la ruta mas corta al sistema de enrutamiento
    async getRoute(wayPoints) {
        //console.log(wayPoints);
        let orderedWayPoints = [];
        const depot = this.depot;
        let url = `https://wse.ls.hereapi.com/2/findsequence.json?apiKey=${this.apikey}` +
            `&start=${this.geotoPos(depot)}` +
            `${this.wayPointMerger(wayPoints)}` +
            `&end=${this.geotoPos(depot)}` +
            `&improveFor=distance&mode=fastest;car;traffic:disabled`
        //console.log(url);
        let distance = 
        fetch(url).then(res => { if (!res.ok) throw Error(res.status); return res })
            .then(res => res.json()).then(data => {
                data.results[0].waypoints.forEach(wp => { orderedWayPoints.push({ lat: parseFloat(wp.lat), lng: parseFloat(wp.lng) }) })
                //console.log(orderedWayPoints);
                return { wayPoints: orderedWayPoints, distance: data.results[0].distance }
            })
            .then(wp => this.drawRoute(wp))
            .then(d => {return d})
            .catch(this.onError)
        return await distance
    }
    clean() {
        this.markers.removeAll()
        this.polylines.removeAll()
    }
    //traduce un objeto posicion individual al sistema api url
    geotoPos(geo) {
        try {
        //    console.log(geo.lng + ',' + geo.lat);
            return geo.lat + ',' + geo.lng
    
        } catch (error) {
            console.error(error, ' wabalabadabdab!');
        }
    }
    //traduce un arreglo de objetos posicion a formato url para peticionar a la api
    wayPointMerger(wayPoints) {
        let result = ''
        wayPoints.forEach((wayPoint, i) => {
            result += `&destination${i + 1}=${this.geotoPos(wayPoint)}`
        });
        return result
    }
    //pide a la api un arreglo de secciones de ruta, recibe un arreglo de coordenadas ordenadas para el viaje mas eficiente
    drawRoute(route) {
        try {
            this.polylines.removeAll()
        } catch (error) {
            console.error('las polilineas se niegan a irse, sera que quieren mas te?')
        }
        var router = this.platform.getRoutingService(null, 8)
        route.wayPoints.forEach((wp, i, arr) => {
            if (arr[i + 1] != undefined) {
                router.calculateRoute(
                    this.routeParams(wp, arr[i + 1]),
                    result => this.drawPoly(result.routes[0]),
                    this.onError
                    );
                }
            
        })
        return route.distance
    }
    //dibuja una polilinea y la añade al mapa, recibe un array ordenado de tramos de ruta
    drawPoly(route) {
        route.sections.forEach((section) => {
            let strokeColor = 'rgba(0, 128, 255, 0.7)' //por ahora es un color fijo, la idea es que cambie segun los tramos sean recorridos (en un futuro lejano)
            let linestring = H.geo.LineString.fromFlexiblePolyline(section.polyline);
            let polyline = new H.map.Polyline(linestring, {
                style: {
                    lineWidth: 4,
                    strokeColor: strokeColor
                }
            })
            this.polylines.addObject(polyline)
        })
    
        this.map.getViewModel().setLookAtData({
            bounds: this.polylines.getBoundingBox()
        })
    }   
    //crea l objeto de configuracion de la route api 
    routeParams(origin, destination) {
        return {
            routingMode: 'fast',
            transportMode: 'car',
            origin: this.geotoPos(origin),
            destination: this.geotoPos(destination),
            return: 'polyline'
        }
    }
    onError(){
        console.error('error en routefunction, un error en este punto puede ser la api de here negandose a responder demasiadas peticiones, por lo general volvera despues de unos comerciales')
    }
}
