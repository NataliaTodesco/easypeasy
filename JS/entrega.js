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

    // Modificar Entrega
    $("#btnModificarEntrega").click(function () {
        let id = $("#cboDetalle").val();
        let observaciones = $("#observacionesModificar").val();

        modificarEntrega(id, observaciones);

    });
    //Eliminar Entrega
    $("#btnEliminar").click(function () {
        let id = $("cboDetalle2").val();
        
        eliminarEntrega(id);

    });

    // GET DETALLE ENTREGA
    $(document).ready(function () {
        $.ajax({
            url: "https://vast-brook-85314.herokuapp.com/Entregas/ObtenerDetalleEntrega",
            type: "GET",
            success: function (result) {
                if (result.ok) {
                    resultadoS = result.return;
                    cargarCombo(resultadoS);
                } else {
                    swal(result.error);
                }
            },
            error: function (error) {
                swal("Problemas al conseguir detalles de entrega");
            },
        })
        // carga combo
        function cargarCombo(datos) {
            var html = "<option value=''>SELECCIONE</option>";
            $("#cboDetalle").append(html);
            select = document.getElementById("cboDetalle");
            for (let i = 0; i < datos.length; i++) {
                var option = document.createElement('option');
                option.value = datos[i].idDetalle;
                option.text = datos[i].horaEntrega;
                select.add(option);
            }
        }

    });
    // GET DETALLE ENTREGA
    $(document).ready(function () {
        $.ajax({
            url: "https://vast-brook-85314.herokuapp.com/Entregas/ObtenerDetalleEntrega",
            type: "GET",
            success: function (result) {
                if (result.ok) {
                    resultadoS = result.return;
                    cargarCombo(resultadoS);
                } else {
                    swal(result.error);
                }
            },
            error: function (error) {
                swal("Problemas al conseguir detalles de entrega");
            },
        })
        // carga combo
        function cargarCombo(datos) {
            var html = "<option value=''>SELECCIONE</option>";
            $("#cboDetalle2").append(html);
            select = document.getElementById("cboDetalle2");
            for (let i = 0; i < datos.length; i++) {
                var option = document.createElement('option');
                option.value = datos[i].idDetalle;
                option.text = datos[i].horaEntrega;
                select.add(option);
            }
        }

    });
}

// ------------ OBTENER INFO REGISTRAR ENTREGAS -------------
function mostrarDatos() {
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemito?id=" + idRemito.value,
        type: "GET",

        success: function (result) {
            if (result.ok) {
                clienteEntrega.value = result.return.cliente.nombre;
                direccionEntrega.value = result.return.cliente.direccion;
                crearTablaDetalles(result.return.productosXRemitos);
                switch (result.return.estado.idEstado) {
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
        html += "<td>" + datos[index].producto.descripcion + "</td>";
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
            html += "<td>" + roundDate(datos[index].fechaCompra) + "</td>";
            html += "<td>" + datos[index].idHojaRutaNavigation.idTransportistaNavigation.nombre + "</td>";
            html += "<td>" +
                "<button type='button' class='btn' id='eliminar' onclick='eliminarEntrega(" + datos[index].idRemito + ")'data-toggle='modal' data-target='#eliminarEntrega' data-placement='bottom'>" +
                "<i class='bi bi-trash-fill'></i> </button>";

            html += "&nbsp; <button type='button' class='btn' id='modificar' onclick='modificarEntrega(" + datos[index].idRemito + ")' data-toggle='modal' data-target='#modificarEntrega' data-placement='bottom'>" +
                "<i class='bi bi-pencil-fill'></i> </button> "

            html += "</td>";
            html += "</tr>"

            $("#cuerpoTablaEntregas").append(html);
        }
    }
}

function roundDate(timeStamp) {
    var yyyy = new Date(timeStamp).getFullYear().toString();
    var mm = new Date(timeStamp).getMonth() + 1;
    var dd = new Date(timeStamp).getDate().toString();
    return dd + "/" + mm + "/" + yyyy;
}

function CargarEntrega() {
    let idRemito = $("#idRemito").val();
    let horaEntrega = $("#horaEntrega").val();
    var firma = null;
    let observaciones = $("#observaciones").val();
    CargarDetalleEntrega(idRemito, horaEntrega, firma, observaciones);
    ActualizarEstadoEntregado(idRemito);
}

function CargarDetalleEntrega(idRemito, horaEntrega, firma, observaciones) {
    comando = {
        "idDetalle": 0,
        "idRemito": parseInt(idRemito),
        "horaEntrega": horaEntrega,
        "firma": firma,
        "observaciones": observaciones
    }

    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/DetalleEntrega/CargarDetalleEntrega", //no está en heroku
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(comando),
        type: "POST",
        success: function (result) {
            if (result.ok) {

            }
            else swal(result.error);

        },
        error: function (error) {
            swal(error);

        }
    });
}


function modificarEntrega(id, observaciones) {
    comando = {
        "idDetalle": parseInt(id),
        "observaciones": observaciones,
    };

    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Entregas/ModificarEntregas",
        type: "PUT",
        dataType: 'JSON',
        contentType: 'application/json',
        data: JSON.stringify(comando),
        success: function (result) {
            if (result.ok) {

            } else {
                swal("Problema Server");
            }
        },
        error: function (error) {
            swal("Error");
        },
    })

}


function eliminarEntrega(id) {


    $.ajax({
        url: "Entregas/DeleteEntrega"+id,//falta heroku
        type: "DELETE",
        success: function (result) {
            if (result.ok) {
                swal("Entrega eliminada");
            }
            else swal(result.error);
        },
        error: function (error) {
            swal("Error");
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