    
   let Clientes = [];
   let cont = [];
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
                crearTabla(result.return.remitos);
                cargarComboId(result.return.remitos);
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
                  option.value = result.return[i].idCliente;
                  select.add(option);
              }
              
              select = document.getElementById("clienteM");
              for (let i = 0; i < result.return.length; i++) {
                  var option = document.createElement('option');
                  option.text = result.return[i].nombre;
                  option.value = result.return[i].idCliente;
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
                    option.value = result.return[i].idEstado;
                    select.add(option);
                }

                select = document.getElementById("estadoM");
                for (let i = 0; i < result.return.length; i++) {
                    var option = document.createElement('option');
                    option.text = result.return[i].descripcion;
                    option.value = result.return[i].idEstado;
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
                else swal("No se encuentra un articulo con ese código")
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
        
        if (datos[index].estado.idEstado == 1) {
        html += "<td>"+ "Pendiente"+"</td>";
        }else if (datos[index].estado.idEstado == 2){
        html += "<td>"+ "En proceso"+"</td>";
        }else if (datos[index].estado.idEstado == 3){
        html += "<td>"+ "Entregado"+"</td>";
        }               
        else if (datos[index].estado.idEstado == 4){
        html += "<td>"+ "Reprogramado"+"</td>";
        }
        else {
            html += "<td>"+ "Cancelado"+"</td>";
            }

        var fecha = roundDate(datos[index].fechaCompra);

        html += "<td>"+ fecha + "</td>";
        html += "<td>"+ datos[index].cliente.nombre + "</td>"; 
        html += "<td>"; 
        
        datos[index].productosXRemitos.forEach(prod => {
            html += prod.producto.descripcion +"<br>";
        }); 

        Clientes.push(datos[index]);
        cont.push(0);
            
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
    var mm = new Date(timeStamp).getMonth()+1;
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
                document.getElementById('nombreE').innerHTML = result.return.cliente.nombre;
                document.getElementById('fechaE').innerHTML = roundDate(result.return.fechaCompra);
                document.getElementById('direccionE').innerHTML = result.return.cliente.direccion
                // " B° "+result.return.cliente.idBarrioNavigation.descripcion+
                // " Zona: "+result.return.cliente.idBarrioNavigation.idZonaNavigation.descripcion;
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
            "horaEntregaPreferido": "16:00",
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
    let fechaR = fecha;
    const hoy = new Date();

    if (idEstado == 4) 
        fechaR =  sumarDias(hoy,1);
    //     comando = {
    //     "idRemito": id,
    //     "fechaCompra": fechaR,
    //     "idEstado": idEstado,
    //     "firma": null,
    //     "observaciones": document.getElementById("observaciones").value,      
          
    // }  

    // if(idEstado==5){
    //     var motivo = document.getElementById('motivos');
    //     var idMotivo = motivo.selectedIndex;
    // comando = {
    //     "idRemito": id,
    //     "fechaCompra": fecha,
    //     "idEstado": idEstado,
    //     "firma": null,
    //     "observaciones": document.getElementById("observaciones").value,       
    //     "idMotivo":idMotivo    
    // }  
       
    //     $.ajax({
    //     //url: "https://localhost:5001/DetalleEntrega/CargarDetalleEntrega",
    //     url: "https://vast-brook-85314.herokuapp.com/DetalleEntrega/CargarDetalleEntrega",
    //     dataType:'json',
    //     contentType:'application/json',
    //     data: JSON.stringify(comando),
    //     type: "POST",
    //     success: function(result) {
    //         if (result.ok){
    //             location.reload();
    //             //swal("Remito modificado exitosamente");
    //         }
    //         else swal(result.error);
    //     },
    //     error: function(error) {
    //         swal("Error" + error);
    //     }
    // });


    // }else{
        comando = {
        "idRemito": id,
        "fechaCompra": fechaR,
        "horaEntregaPreferido": "16:00",
        "idEstado": idEstado,
        "idCliente": idCliente,
        "idHojaRuta": 0        
    }
        $.ajax({
        //url: "https://localhost:5001/Remito/Actualizar",
        url: "https://vast-brook-85314.herokuapp.com/Remito/Actualizar",
        
        dataType:'json',
        contentType:'application/json',
        data: JSON.stringify(comando),
        type: "PUT",
        success: function(result) {
            if (result.ok){
                location.reload();
                //swal("Remito modificado exitosamente");
            }
            else swal(result.error);
        },
        error: function(error) {
            swal("Error" + error);
        }
    });

    }

  
  //}

  function sumarDias(fecha, dias){
    fecha.setDate(fecha.getDate() + dias);
    return fecha;
  }

  function modificar(id) {
    let idRem = id;
    let idCl = 0;
    let fecha = "";
    let idE = 0;

    for (let i = 0; i < Clientes.length; i++) {
        if (Clientes[i].idRemito == id){
        idCl = Clientes[i].cliente.idCliente
        fecha = Clientes[i].fechaCompra;
        idE = Clientes[i].estado.idEstado;
        
        }
    }
    
    $("#clienteM").val(idCl);
    //$("#fechaM").val(fecha);
    document.getElementById('fechaM').valueAsDate = new Date();
    // if (idE == 4)
    // $("#contador").val(1);
    // else $("#contador").val(0);
    
    // $("#estadoM").val(idE);

    $("#btnModificar").click(function() {    
        let fecha = $("#fechaM").val();
        var hoy = new Date();
        let hora = hoy.getHours() + ':' + hoy.getMinutes();
        var cliente = document.getElementById('clienteM');
        var idCliente = cliente.selectedIndex;
        var estado = document.getElementById('estadoM');
        var idEstado = estado.selectedIndex;
        let c = Number($("#contador").val());

        // if (idEstado == 4) $("#contador").val(c++);
        // if (c == 3) swal("La entrega ya fue reprogramada 2(Dos) veces, por favor registrar como cancelación");
        // else
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
        let ingresos = [];

        function agregar() {
            const formulario = document.getElementById('formulario');
            let descripcion = formulario['producto'];
            let valor = formulario['valor'];
            

            var prod = document.getElementById('producto');
            var idProducto = prod.selectedIndex;

            var lista = {
                descripcion: descripcion.value,
                cantidad: Number(valor.value)
            }

            let elemento = ingresos.find(element => element.descripcion == descripcion.value);

            let cant = 0;

            try {
                elemento = {
                    descripcion: elemento.descripcion,
                    cantidad: Number(elemento.cantidad += Number(lista.cantidad))
                }
                cant = elemento.cantidad;
            } catch (error) {
                ingresos.push(lista);
                cant = Number(valor.value);
            }


            let detalle = "";

            for (let i = 0; i < ingresos.length; i++) {
                detalle += ingresos[i].descripcion +"&nbsp &nbsp &nbsp &nbsp &nbsp Cant. "+ ingresos[i].cantidad+" <hr>";
            }

            document.getElementById('itemIngreso').innerHTML = detalle;

            let cant2 = ingresos.find(element => element.descripcion == descripcion.value);

            if (cant != 0)
            cargarProducto(idProducto,cant2.cantidad);
            else swal("Ingrese Cantidad");
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

              if (fecha != "" & idCliente != 0){
                CargarFormulario(fecha,hora,idCliente,idEstado,Productos);
              }
              else {
                  swal("Por favor complete todos los campos")
              }
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
                        //location.reload();
                        window.location.href = "./remitos.html";
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
        let fecha = $("#fechaB").val();
        let estado = document.getElementById('estado');
        let idEstado = estado.selectedIndex;

        if (id == 0 && fecha == "" && idEstado == 0){
            $("#cuerpoTabla").show();
            $("#cuerpoTablaID").hide();
            $("#cuerpoTablaFecha").hide();
            $("#cuerpoTablaEstado").hide();
            $("#cuerpoTablaFE").hide();
        } else if (id != 0 && fecha == "" && idEstado == 0){
            $.ajax({
                url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemito?id=" + id,
                type: "GET",
                success: function (result) {
                    if (result.ok) {
                        $("#cuerpoTablaID").empty();
                        $("#cuerpoTablaID").show()

                        let html = "<tr id='lista'>";
                
                        html += "<td>"+result.return.idRemito+"</td>";
                        
                        if (result.return.estado.idEstado == 1) {
                        html += "<td>"+ "Pendiente"+"</td>";
                        }else if (result.return.estado.idEstado == 2){
                        html += "<td>"+ "En proceso"+"</td>";
                        }else if (result.return.estado.idEstado == 3){
                        html += "<td>"+ "Entregado"+"</td>";
                        }               
                        else if (result.return.estado.idEstado == 4){
                        html += "<td>"+ "Reprogramado"+"</td>";
                        }
                        else {
                            html += "<td>"+ "Cancelado"+"</td>";
                            }
                
                        html += "<td>"+ roundDate(result.return.fechaCompra) + "</td>";
                        html += "<td>"+ result.return.cliente.nombre + "</td>"; 
                        html += "<td>"; 
                        
                        result.return.productosXRemitos.forEach(prod => {
                            html += prod.producto.descripcion +"<br>";
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
                        $("#cuerpoTablaFecha").hide();
                        $("#cuerpoTablaEstado").hide();
                        $("#cuerpoTablaFE").hide();
                            
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
    } else if (id == 0 && fecha != "" && idEstado == 0){
        $.ajax({
            url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemitoXFecha?fecha=" + fecha,
            type: "GET",
            success: function (result) {
                $("#cuerpoTablaFecha").empty();
                $("#cuerpoTablaFecha").show()
                let html = "<tr id='lista'>";

                if (result.ok) {
                    for (let index = 0; index < result.return.remitos.length; index++) {
                        html += "<td>"+result.return.remitos[index].idRemito+"</td>";
                        
                        if (result.return.remitos[index].estado.idEstado == 1) {
                        html += "<td>"+ "Pendiente"+"</td>";
                        }else if (result.return.remitos[index].estado.idEstado == 2){
                        html += "<td>"+ "En proceso"+"</td>";
                        }else if (result.return.remitos[index].estado.idEstado == 3){
                        html += "<td>"+ "Entregado"+"</td>";
                        }               
                        else if (result.return.remitos[index].estado.idEstado == 4){
                            html += "<td>"+ "Reprogramado"+"</td>";
                            }
                            else {
                                html += "<td>"+ "Cancelado"+"</td>";
                                }

                        var fecha = roundDate(result.return.remitos[index].fechaCompra);

                        html += "<td>"+ fecha + "</td>";
                        html += "<td>"+ result.return.remitos[index].cliente.nombre + "</td>"; 
                        html += "<td>"; 
                        
                        result.return.remitos[index].productosXRemitos.forEach(prod => {
                            html += prod.producto.descripcion +"<br>";
                        }); 
                            
                        html += "</td>";  

                        html += "<td>"+ "<button type='button' class='btn' id='eliminar' onclick='eliminar("+result.return.remitos[index].idRemito+")' data-placement='bottom'>"+
                                "<i class='bi bi-trash-fill'></i> </button> &nbsp;"+
                                "<button type='button' class='btn' id='modificar' onclick='modificar("+result.return.remitos[index].idRemito+")' data-toggle='modal' data-target='#modificarM' data-placement='bottom'>"+
                                "<i class='bi bi-pencil-fill'></i> </button> &nbsp;"+
                                "<button type='button' class='btn' id='etic' onclick='generarEtiqueta("+result.return.remitos[index].idRemito+")' data-toggle='modal' data-target='#etiqueta' data-placement='bottom'>"+
                                "<i class='bi bi-file-text'></i> </button> " + "</td>";
                        html += "</tr>"
                    }

                    $("#cuerpoTabla").hide();
                    $("#cuerpoTablaID").hide();
                    $("#cuerpoTablaEstado").hide();
                    $("#cuerpoTablaFE").hide();
                        
                    $("#cuerpoTablaFecha").append(html);

                    swal("Datos traidos con exito");
                } else {
                    swal(result.error);
                }
            },
            error: function (error) {
                swal("Problemas en el servidor");
            },
        })
    } else if (id == 0 && fecha == "" && idEstado != 0){
        $.ajax({
            url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemitoXEstado?estado=" + idEstado,
            type: "GET",
            success: function (result) {
                if (result.ok) {
                    $("#cuerpoTablaEstado").empty();
                    $("#cuerpoTablaEstado").show()

                    let html = "<tr id='lista'>";
            
                    for (let index = 0; index < result.return.remitos.length; index++) {
                        html += "<td>"+result.return.remitos[index].idRemito+"</td>";
                        
                        if (result.return.remitos[index].estado.idEstado == 1) {
                        html += "<td>"+ "Pendiente"+"</td>";
                        }else if (result.return.remitos[index].estado.idEstado == 2){
                        html += "<td>"+ "En proceso"+"</td>";
                        }else if (result.return.remitos[index].estado.idEstado == 3){
                        html += "<td>"+ "Entregado"+"</td>";
                        }               
                        else if (result.return.remitos[index].estado.idEstado == 4){
                            html += "<td>"+ "Reprogramado"+"</td>";
                            }
                            else {
                                html += "<td>"+ "Cancelado"+"</td>";
                                }

                        var fecha = roundDate(result.return.remitos[index].fechaCompra);

                        html += "<td>"+ fecha + "</td>";
                        html += "<td>"+ result.return.remitos[index].cliente.nombre + "</td>"; 
                        html += "<td>"; 
                        
                        result.return.remitos[index].productosXRemitos.forEach(prod => {
                            html += prod.producto.descripcion +"<br>";
                        }); 
                            
                        html += "</td>";  

                        html += "<td>"+ "<button type='button' class='btn' id='eliminar' onclick='eliminar("+result.return.remitos[index].idRemito+")' data-placement='bottom'>"+
                                "<i class='bi bi-trash-fill'></i> </button> &nbsp;"+
                                "<button type='button' class='btn' id='modificar' onclick='modificar("+result.return.remitos[index].idRemito+")' data-toggle='modal' data-target='#modificarM' data-placement='bottom'>"+
                                "<i class='bi bi-pencil-fill'></i> </button> &nbsp;"+
                                "<button type='button' class='btn' id='etic' onclick='generarEtiqueta("+result.return.remitos[index].idRemito+")' data-toggle='modal' data-target='#etiqueta' data-placement='bottom'>"+
                                "<i class='bi bi-file-text'></i> </button> " + "</td>";
                        html += "</tr>"
                    }

                    $("#cuerpoTabla").hide();
                    $("#cuerpoTablaID").hide();
                    $("#cuerpoTablaFecha").hide();
                    $("#cuerpoTablaFE").hide();
                        
                    $("#cuerpoTablaEstado").append(html);

                    swal("Datos traidos con exito");
                } else {
                    swal(result.error);
                }
            },
            error: function (error) {
                swal("Problemas en el servidor");
            },
        })
    } else if (id == 0 && fecha != "" && idEstado != 0){
        $.ajax({
            url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemitoXFechaYEstado?estado="+idEstado+"&fecha="+fecha,
            type: "GET",
            success: function (result) {
                if (result.ok) {
                    $("#cuerpoTablaFE").empty();
                    $("#cuerpoTablaFE").show()

                    let html = "<tr id='lista'>";
            
                    for (let index = 0; index < result.return.remitos.length; index++) {
                        html += "<td>"+result.return.remitos[index].idRemito+"</td>";
                        
                        if (result.return.remitos[index].estado.idEstado == 1) {
                        html += "<td>"+ "Pendiente"+"</td>";
                        }else if (result.return.remitos[index].estado.idEstado == 2){
                        html += "<td>"+ "En proceso"+"</td>";
                        }else if (result.return.remitos[index].estado.idEstado == 3){
                        html += "<td>"+ "Entregado"+"</td>";
                        }               
                        else if (result.return.remitos[index].estado.idEstado == 4){
                            html += "<td>"+ "Reprogramado"+"</td>";
                            }
                            else {
                                html += "<td>"+ "Cancelado"+"</td>";
                                }

                        var fecha = roundDate(result.return.remitos[index].fechaCompra);

                        html += "<td>"+ fecha + "</td>";
                        html += "<td>"+ result.return.remitos[index].cliente.nombre + "</td>"; 
                        html += "<td>"; 
                        
                        result.return.remitos[index].productosXRemitos.forEach(prod => {
                            html += prod.producto.descripcion +"<br>";
                        }); 
                            
                        html += "</td>";  

                        html += "<td>"+ "<button type='button' class='btn' id='eliminar' onclick='eliminar("+result.return.remitos[index].idRemito+")' data-placement='bottom'>"+
                                "<i class='bi bi-trash-fill'></i> </button> &nbsp;"+
                                "<button type='button' class='btn' id='modificar' onclick='modificar("+result.return.remitos[index].idRemito+")' data-toggle='modal' data-target='#modificarM' data-placement='bottom'>"+
                                "<i class='bi bi-pencil-fill'></i> </button> &nbsp;"+
                                "<button type='button' class='btn' id='etic' onclick='generarEtiqueta("+result.return.remitos[index].idRemito+")' data-toggle='modal' data-target='#etiqueta' data-placement='bottom'>"+
                                "<i class='bi bi-file-text'></i> </button> " + "</td>";
                        html += "</tr>"
                    }

                    $("#cuerpoTabla").hide();
                    $("#cuerpoTablaID").hide();
                    $("#cuerpoTablaFecha").hide();
                    $("#cuerpoTablaEstado").hide();
                        
                    $("#cuerpoTablaFE").append(html);

                    swal("Datos traidos con exito");
                } else {
                    swal(result.error);
                }
            },
            error: function (error) {
                swal("Problemas en el servidor");
            },
        })
    }else {
        swal("No se puede buscar por N°, Fecha y Estado, por favor borre los campos que no requiera");
    }
}

        // https://vast-brook-85314.herokuapp.com/swagger/index.html