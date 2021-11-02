    
    // Validar Form
    function go() {
        var forms = document.getElementsByClassName('needs-validation');

        var validation = Array.prototype.filter.call(forms, function(form) {
        form.addEventListener('submit', function(event) {
            if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
        });
    }

    function OnLoad() {     
    // Tooltip
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
    
    // Obtener Remitos
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemitos",
        type: "GET",
        
        success: function (result) {
            if (result.ok) {
                crearTabla(result.return);
                cargarComboId(result.return);
            }else{
                swal(result.error);
            }
        },
        error: function (error) {
            console.log();(error);
        },
    });

    //Obtener Productos
    $.ajax({
          url: "https://vast-brook-85314.herokuapp.com/Producto/ObtenerProducto",
          type: "GET",
          success: function(result) {
              crearTablaA(result.return);
              select = document.getElementById("producto");
              for (let i = 0; i < result.return.length; i++) {
                  var option = document.createElement('option');
                  option.text = result.return[i].descripcion;
                  select.add(option);
              }
          },
          error: function(error) {
              swal(result.error);
          }
      });

    // Obtener Clientes
      $.ajax({
          url: "https://vast-brook-85314.herokuapp.com/Cliente/ObtenerCliente",
          type: "GET",
          success: function(result) {
              select = document.getElementById("cliente");
              for (let i = 0; i < result.return.length; i++) {
                  var option = document.createElement('option');
                  option.text = result.return[i].nombre;
                  select.add(option);
              }
              
              select = document.getElementById("clienteM");
              for (let i = 0; i < result.return.length; i++) {
                  var option = document.createElement('option');
                  option.text = result.return[i].nombre;
                  select.add(option);
              }
          },
          error: function(error) {
              swal(result.error);
          }
      });

    // Obtener Estados
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Estado/ObtenerEstado",
        type: "GET",
        
        success: function (result) {
            if (result.ok) {
                select = document.getElementById("estado");
                var option = document.createElement('option');
                option.text = "Seleccionar...";
                select.add(option);

                for (let i = 0; i < result.return.length; i++) {
                    var option = document.createElement('option');
                    option.text = result.return[i].descripcion;
                    select.add(option);
                }

                select = document.getElementById("estadoM");
                for (let i = 0; i < result.return.length; i++) {
                    var option = document.createElement('option');
                    option.text = result.return[i].descripcion;
                    select.add(option);
                }
            }else{
                swal(result.error);
            }
        },
        error: function (error) {
            console.log();(error);
        },
    });

    // Obtener Articulo por ID 
    $("#buscar").click(function() {
        let id = $("#id").val();

        $.ajax({
          url: "https://vast-brook-85314.herokuapp.com/Producto/ObtenerUnProducto?id="+id,
          type: "GET",
          success: function(result) {
            if (result.ok){
                if (result.return != null)
                    document.getElementById('articulo').innerHTML = "Articulo: "+ result.return.idProducto+" - "+result.return.descripcion;
                else swal("No se encuentra un articulo con ese ID")
            }
            else swal(result.error);
          },
          error: function(error) {
            swal(error);
          }
        });
      })

    // Cargar ProductoXRemito
      $("#btnAceptar").click(function(){
        swal("Productos registrados","Continue con la carga del remito","success")
        cargarRemito(Productos);
      })
}

    //Crear tabla Remitos
  function crearTabla(datos) {
    for (let index = 0; index < datos.length; index++) {
        let html = "<tr id='lista'>";
        
        html += "<td>"+datos[index].idRemito+"</td>";
        
        if (datos[index].idEstado == 1) {
        html += "<td>"+ "Pendiente"+"</td>";
        }else if (datos[index].idEstado == 2){
        html += "<td>"+ "En proceso"+"</td>";
        }else if (datos[index].idEstado == 3){
        html += "<td>"+ "Entregado"+"</td>";
        }               
        else {
        html += "<td>"+ "Reprogramado"+"</td>";
        }

        var fecha = roundDate(datos[index].fechaCompra);

        html += "<td>"+ fecha + "</td>";
        html += "<td>"+ datos[index].idClienteNavigation.nombre + "</td>"; 
        html += "<td>"; 
        
        datos[index].productosXremitos.forEach(prod => {
            html += prod.idProductoNavigation.descripcion +"<br>";
        }); 
            
        html += "</td>";  

        html += "<td>"+ "<button type='button' class='btn' id='eliminar' onclick='eliminar("+datos[index].idRemito+")' data-placement='bottom'>"+
                "<i class='bi bi-trash-fill'></i> </button> &nbsp;"+
                "<button type='button' class='btn' id='modificar' onclick='modificar("+datos[index].idRemito+")' data-toggle='modal' data-target='#modificarM' data-placement='bottom'>"+
                "<i class='bi bi-pencil-fill'></i> </button> &nbsp;"+
                "<button type='button' class='btn' id='etic' onclick='generarEtiqueta("+datos[index].idRemito+")' data-toggle='modal' data-target='#etiqueta' data-placement='bottom'>"+
                "<i class='bi bi-file-text'></i> </button> " + "</td>";
        html += "</tr>"
      
        $("#cuerpoTabla").append(html);
    } 
  }

  function roundDate(timeStamp){
    var yyyy = new Date(timeStamp).getFullYear().toString();
    var mm = new Date(timeStamp).getMonth().toString();
    var dd  = new Date(timeStamp).getDate().toString();
    return dd +"/"+ mm +"/" + yyyy;
  }

  // Generar Etiqueta
  function generarEtiqueta(id){
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemito?id="+id,
        type: "GET",
        success: function(result) {
            if (result.ok){
                document.getElementById('nombreE').innerHTML = result.return.idClienteNavigation.nombre;
                document.getElementById('fechaE').innerHTML = roundDate(result.return.fechaCompra);
                document.getElementById('direccionE').innerHTML = result.return.idClienteNavigation.direccion+
                " B° "+result.return.idClienteNavigation.idBarrioNavigation.descripcion+
                " Zona: "+result.return.idClienteNavigation.idBarrioNavigation.idZonaNavigation.descripcion;
                document.getElementById('numeroE').innerHTML = result.return.idRemito;
            }
            else swal(result.error);
        },
        error: function(error) {
            swal("Error" + error);
        }
    });
   
    
 }


  // Crear tabla Articulos
  function crearTablaA(datos) {
    for (let index = 0; index < datos.length; index++) {
        let html = "<tr>"; 
        html += "<td>" + datos[index].idProducto + "</td>";             
        html += "<td>"+ datos[index].descripcion + "</td>";
        html += "</tr>"
      
        $("#cuerpoTablaA").append(html);
    } 
  }

  // Cargar Remitos
    function CargarFormulario(fecha,hora,idCliente,idEstado,Productos) {
        comando = {
            "idRemito": 0,
            "fechaCompra": fecha,
            "horaEntregaPreferido": hora,
            "idEstado": idEstado,
            "idCliente": idCliente,
            "idHojaRuta": 0
        }

        $.ajax({
            url: "https://vast-brook-85314.herokuapp.com/Remito/CargarRemito",
            dataType:'json',
            contentType:'application/json',
            data: JSON.stringify(comando),
            type: "POST",
            success: function(result) {
                if (result.ok){
                    IngresarProd(result.return.idRemito,Productos)
                    swal("Remito cargado exitosamente");
                }
                else swal(result.error);
            },
            error: function(error) {
                swal(error);
            }
        });
    }

  // Modificar Remitos
  function ModificarFormulario(id,fecha,hora,idCliente,idEstado) {
    comando = {
        "idRemito": id,
        "fechaCompra": fecha,
        "horaEntregaPreferido": hora,
        "idEstado": idEstado,
        "idCliente": idCliente,
        "idHojaRuta": 0
    }

    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Estado/ObtenerEstado",
        dataType:'json',
        contentType:'application/json',
        data: JSON.stringify(comando),
        type: "PUT",
        success: function(result) {
            if (result.ok){
                swal("Remito modificado exitosamente");
            }
            else swal(result.error);
        },
        error: function(error) {
            swal("Error" + error);
        }
    });
  }

  function modificar(id) {
    let idRem = id;

    $("#btnModificar").click(function() {    
        let fecha = $("#fechaM").val();
        var hoy = new Date();
        let hora = hoy.getHours() + ':' + hoy.getMinutes();
        var cliente = document.getElementById('clienteM');
        var idCliente = cliente.selectedIndex;
        var estado = document.getElementById('estadoM');
        var idEstado = estado.selectedIndex;

        ModificarFormulario(idRem,fecha,hora,idCliente,idEstado);
    })
  }

  // Eliminar
  function eliminar(id) {
    var lista = document.getElementById("cuerpoTabla")

     var eleminarTarea = function(){
         this.parentNode.removeChild(this);
     };

     for (var i = 0; i <= lista.children.length -1; i++) {
         lista.children[i].addEventListener("click", eleminarTarea);
     } 
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/RemXProd/EliminarRemXProd?id="+id,
        type: "DELETE",
        success: function(result) {
            if (result.ok){
                swal("Remito eliminado exitosamente");
            }
            else swal(result.error);
        },
        error: function(error) {
            swal("Error" + error);
        }
    });

     $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/EliminarRemito?id="+id,
        type: "DELETE",
        success: function(result) {
            if (result.ok){
                swal("Remito eliminado exitosamente");
            }
            else swal(result.error);
        },
        error: function(error) {
            swal("Error" + error);
        }
    });


    swal("Elemento eliminado");
  }

  // Cargar Productos X Remitos
        let Productos = [];

        function agregar() {
            const formulario = document.getElementById('formulario');
            let descripcion = formulario['producto'];
            let valor = formulario['valor'];
            let ingresos = [];

            var prod = document.getElementById('producto');
            var idProducto = prod.selectedIndex;

            ingresos.push(descripcion.value +"&nbsp &nbsp &nbsp &nbsp &nbsp Cant. "+valor.value+" <hr>");
            document.getElementById('itemIngreso').innerHTML += ingresos;

            cargarProducto(idProducto,valor.value)
        }

        function cargarProducto(index,cant){
          producto = {
            indice : index,
            cantidad : Number(cant)
          };

          Productos.push(producto);
        }
        
        function cargarRemito(Productos){
          $("#btnI").click(function() {
              let fecha = $("#fecha").val();
              var hoy = new Date();
              let hora = hoy.getHours() + ':' + hoy.getMinutes();
              var cliente = document.getElementById('cliente');
              var idCliente = cliente.selectedIndex;
              var idEstado = 1;

              CargarFormulario(fecha,hora,idCliente,idEstado,Productos);
          })
        }

        function IngresarProd(idRemito, Productos){
          for (let i = 0; i < Productos.length; i++) {
            comando = {
                "idProducto": Productos[i].indice,
                "idRemito": idRemito,
                "cantidad": Productos[i].cantidad
            }

            $.ajax({
                url: "https://vast-brook-85314.herokuapp.com/RemXProd/RegistrarRemXProd",
                dataType:'json',
                contentType:'application/json',
                data: JSON.stringify(comando),
                type: "POST",
                success: function(result) {
                    if (result.ok){
                        console.log("ProdxRemito cargado exitosamente");
                    }
                    else swal(result.error);
                },
                error: function(error) {
                    swal(error);
                }
            });
            
          }
        }

    function cargarComboId(datos) {
        var html = "<option value=''>Seleccione ID...</option>";
        $("#cboId").append(html);
        select = document.getElementById("cboId");
        for (let i = 0; i < datos.length; i++) {
            var option = document.createElement('option');
            option.value = datos[i].idRemito;
            option.text = datos[i].idRemito;
            select.add(option);
        }
    }

    function buscar(){
        let id = Number($("#cboId").val());

        if (id == 0){
            $("#cuerpoTabla").show();
            $("#cuerpoTablaID").hide();
        }else{

        $.ajax({
            url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemito?id=" + id,
            type: "GET",
            success: function (result) {
                if (result.ok) {
                    $("#cuerpoTablaID").empty();
                    $("#cuerpoTablaID").show()

                    let html = "<tr id='lista'>";
            
                    html += "<td>"+result.return.idRemito+"</td>";
                    
                    if (result.return.idEstado == 1) {
                    html += "<td>"+ "Pendiente"+"</td>";
                    }else if (result.return.idEstado == 2){
                    html += "<td>"+ "En proceso"+"</td>";
                    }else if (result.return.idEstado == 3){
                    html += "<td>"+ "Entregado"+"</td>";
                    }               
                    else {
                    html += "<td>"+ "Reprogramado"+"</td>";
                    }
            
                    
            
                    html += "<td>"+ result.return.fechaCompra + "</td>";
                    html += "<td>"+ result.return.idClienteNavigation.nombre + "</td>"; 
                    html += "<td>"; 
                    
                    result.return.productosXremitos.forEach(prod => {
                        html += prod.idProductoNavigation.descripcion +"<br>";
                    }); 
                        
                    html += "</td>";  
            
                    html += "<td>"+ "<button type='button' class='btn' id='eliminar' onclick='eliminar("+result.return.idRemito+")' data-placement='bottom'>"+
                            "<i class='bi bi-trash-fill'></i> </button> &nbsp;"+
                            "<button type='button' class='btn' id='modificar' onclick='modificar("+result.return.idRemito+")' data-toggle='modal' data-target='#modificarM' data-placement='bottom'>"+
                            "<i class='bi bi-pencil-fill'></i> </button> &nbsp;"+
                            "<button type='button' class='btn' id='etic' onclick='generarEtiqueta("+result.return.idRemito+")' data-toggle='modal' data-target='#etiqueta' data-placement='bottom'>"+
                            "<i class='bi bi-file-text'></i> </button> " + "</td>";
                    html += "</tr>"

                    $("#cuerpoTabla").hide();
                
                    $("#cuerpoTablaID").append(html);

                    swal("Datos traidos con exito");
                } else {
                    swal(result.error);
                }
            },
            error: function (error) {
                swal("Problemas en el servidor");
            },
        })
    }
}


        // ObtenerRemitoXEstado

        // https://vast-brook-85314.herokuapp.com/swagger/index.html