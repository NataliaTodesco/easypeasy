    
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

    // Obtener Estados Cambiar URL
    $.ajax({
        url: "https://localhost:5001/Estado/ObtenerEstado",
        type: "GET",
        
        success: function (result) {
            if (result.ok) {
                select = document.getElementById("estado");
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

    // Obtener Articulo por ID Cambiar URL
    $("#buscar").click(function() {
        let id = $("#id").val();

        $.ajax({
          url: "https://localhost:5001/Producto/ObtenerUnProducto?id="+id,
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

    // Cargar Remitos
    $("#btnI").click(function() {
        let fecha = $("#fecha").val();
        let hora = $("#hora").val();
        var cliente = document.getElementById('cliente');
        var idCliente = cliente.selectedIndex;
        var estado = document.getElementById('estado');
        var idEstado = estado.selectedIndex;

        CargarFormulario(fecha,hora,idCliente,idEstado);
    })

    // Modificar Remitos
    $("#btnModificar").click(function() {
        let id = $("#idRemito").val();
        let fecha = $("#fechaM").val();
        let hora = $("#horaM").val();
        var cliente = document.getElementById('clienteM');
        var idCliente = cliente.selectedIndex;
        var estado = document.getElementById('estadoM');
        var idEstado = estado.selectedIndex;

        ModificarFormulario(id,modificar,fecha,hora,idCliente,idEstado);
    })
}

    //Crear tabla Remitos
  function crearTabla(datos) {
    for (let index = 0; index < datos.length; index++) {
        let html = "<tr id='lista'>";
        
        html += "<td> <input type='number' id='idRemito' value='"+datos[index].idRemito+"' disabled> </td>";
        
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

        html += "<td>"+ datos[index].fechaCompra + "</td>";
        html += "<td>"+ datos[index].idClienteNavigation.nombre + "</td>";    
        html += "<td>"+ "<button type='button' class='btn' id='eliminar' onclick='eliminar()' data-placement='bottom' title='Generar etiqueta de envío'>"+
                "<i class='bi bi-trash-fill'></i> </button> &nbsp;"+
                "<button type='button' class='btn' id='modificar' data-toggle='modal' data-target='#modificarM' data-placement='bottom' title='Generar etiqueta de envío'>"+
                "<i class='bi bi-pencil-fill'></i> </button> &nbsp;"+
                "<button type='button' class='btn' id='etic' data-toggle='modal' data-target='#etiqueta' data-placement='bottom' title='Generar etiqueta de envío'>"+
                "<i class='bi bi-file-text'></i> </button> " + "</td>";
        html += "</tr>"
      
        $("#cuerpoTabla").append(html);
    } 
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
    function CargarFormulario(fecha,hora,idCliente,idEstado) {
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
        url: "https://vast-brook-85314.herokuapp.com/Remito/Actualizar",
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

  // Eliminar
  function eliminar() {
    var lista = document.getElementById("cuerpoTabla")

     var eleminarTarea = function(){
         this.parentNode.removeChild(this);
     };

     for (var i = 0; i <= lista.children.length -1; i++) {
         lista.children[i].addEventListener("click", eleminarTarea);
     }

    swal("Elemento eliminado");
  }
