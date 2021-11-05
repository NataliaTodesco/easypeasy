// Validar Form
function go() {
    var forms = document.getElementsByClassName('needs-validation');

    var validation = Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
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

    idRemito = document.getElementById("idRemito");
    clienteEntrega = document.getElementById('clienteEntrega');
    direccionEntrega = document.getElementById('direccionEntrega');
    productoEntrega = document.getElementById('productoEntrega');
    cantidadEntrega = document.getElementById('cantidadEntrega');
    estadoEntrega = document.getElementById("estadoEntrega");

    // ----------- OBTENER REMITOS  --------------
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerDetallesRemitos", //falta agregarla al heroku
        type: "GET",

        success: function (result) {
            if (result.ok) {
                for (let i = 0; i < result.return.length; i++) {
                    //---- OBTENER REMITOS NO ENTREGADOS
                    if (result.return[i].idEstado != 2) {
                        var option = document.createElement('option');
                        option.text = result.return[i].idRemito;
                        idRemito.add(option);
                    }
                }
                crearTablaEntregas(result.return);
            } else {
                swal(result.error);
            }
        },
        error: function (error) {
            console.log();
            (error);
        },
    });
}

// ------------ OBTENER INFO REGISTRAR ENTREGAS -------------
function mostrarDatos() {
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemito?id=" + idRemito.value,
        type: "GET",

        success: function (result) {
            if (result.ok) {
                clienteEntrega.value = result.return.idClienteNavigation.nombre;
                direccionEntrega.value = result.return.idClienteNavigation.direccion;
                crearTablaDetalles(result.return.productosXremitos);
                switch (result.return.idEstado) {
                    case 1:
                        estadoEntrega.value = "Pendiente";
                        break;
                    case 2:
                        estadoEntrega.value = "En proceso";
                        break;
                    case 3:
                        estadoEntrega.value = "Entregado";
                        break;
                    case 4:
                        estadoEntrega.value = "Reprogramar";
                        break;
                
                    default: estadoEntrega.value = "---";
                        break;
                }
            } else {
                swal(result.error);
            }
        },
        error: function (error) {
            console.log();
            (error);
        },
    });
};

// --------- CREAR TABLA DETALLES CARGAR ENTREGA --------
function crearTablaDetalles(datos) {
    $("#cuerpoTabla tr").remove();
    for (let index = 0; index < datos.length; index++) {
        let html = "<tr>";
        html += "<td>" + datos[index].idProductoNavigation.descripcion + "</td>";
        html += "<td>" + datos[index].cantidad + "</td>";
        html += "</tr>"

        $("#cuerpoTabla").append(html);
    }
}

// ---------- CREAR TABLA ENTREGAS -----------
function crearTablaEntregas(datos) {
    $("#cuerpoTablaEntregas tr").remove();
    //codigo, estado, fecha, transpotista
    for (let index = 0; index < datos.length; index++) {
        if (datos[index].idEstado == 1) {

            let html = "<tr>";
            html += "<td>" + datos[index].idRemito + "</td>";
            html += "<td>" + datos[index].idEstadoNavigation.descripcion + "</td>";
            html += "<td>" + datos[index].fechaCompra + "</td>";
            html += "<td>" + datos[index].idHojaRutaNavigation.idTransportistaNavigation.nombre + "</td>";
            html += "<td>" +
                "<button type='button' class='btn' id='eliminar' onclick='eliminarEntrega(" + datos[index].idRemito + ")' data-placement='bottom'>" +
                "<i class='bi bi-trash-fill'></i> </button>";

            html += "&nbsp; <button type='button' class='btn' id='modificar' onclick='modificarEntrega(" + datos[index].idRemito + ")' data-toggle='modal' data-target='#modificarEntrega' data-placement='bottom'>" +
                "<i class='bi bi-pencil-fill'></i> </button> "

            html += "</td>";
            html += "</tr>"

            $("#cuerpoTablaEntregas").append(html);
        }
    }
}

    function CargarEntrega() {
        let idRemito = $("#idRemito").val();
            let horaEntrega = $("#horaEntrega").val();
            var firma = null;
            let observaciones = $("#observaciones").val();
            CargarDetalleEntrega(idRemito,horaEntrega,firma,observaciones);
        ActualizarEstadoEntregado(idRemito);
    }

function CargarDetalleEntrega(idRemito,horaEntrega,firma,observaciones) {
    comando = {
        "IdDetalle": 0,
        "IdRemito": parseInt(idRemito),
        "HoraEntrega": horaEntrega,
        "Firma": firma,
        "Observaciones": observaciones
    }

    $.ajax({
        url: "https://localhost:5001/DetalleEntrega/CargarDetalleEntrega", //no está en heroku
        dataType:'json',
        contentType:'application/json',
        data: JSON.stringify(comando),
        type: "POST",
        success: function(result) {
            if (result.ok){
                
            }
            else swal(result.error);
            
        },
        error: function(error) {
            swal(error);
            
        }
    });
    
}

// function ActualizarEstado(id) {

//     $.ajax({
//         url: "https://vast-brook-85314.herokuapp.com/Remito/Actualizar",
//         dataType:'json',
//         contentType:'application/json',
//         data: JSON.stringify(id),
//         type: "PUT",
//         success: function(result) {
//             if (result.ok){
//                 // location.reload();
//                 swal("Remito modificado exitosamente");
//             }
//             else swal(result.error);
//         },
//         error: function(error) {
//             swal("Error" + error);
//         }
//     });
//   }
// https://vast-brook-85314.herokuapp.com/swagger/index.html