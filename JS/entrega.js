let Clientes = [];
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
                    if (result.return[i].idEstado != 3 && result.return[i].idEstado != 5) {
                        var option = document.createElement('option');
                        option.text = result.return[i].idRemito;
                        idRemito.add(option);
                    }
                }
                crearTablaEntregas(result.return);
                var html = "<option value='0'>Seleccione...</option>";
                $("#nroEntrega").append(html);
                select = document.getElementById("nroEntrega");
                for (let i = 0; i < result.return.length; i++) {
                    if (result.return[i].idEstado == 3) {
                        var option = document.createElement('option');
                        option.value = result.return[i].idRemito;
                        option.text = result.return[i].idRemito;
                        select.add(option);
                    }
                }

            } else {
                console.log(result.error);
            }
        },
        error: function (error) {
            console.log();
            (error);
        },
    });

    // Modificar Entrega
    $("#btnModificarEntrega").click(function () {
        let id = $("#cboDetalle2").val();
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
                    console.log(result.error);
                }
            },
            error: function (error) {
                swal("Problemas al conseguir detalles de entrega");
            },
        })
        // carga combo
        function cargarCombo(datos) {
            var html = "<option value='0'>SELECCIONE</option>";
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
                    console.log(result.error);
                }
            },
            error: function (error) {
                swal("Problemas al conseguir detalles de entrega");
            },
        })
        // carga combo
        function cargarCombo(datos) {
            var html = "<option value='0'>SELECCIONE</option>";
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



    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Cliente/ObtenerCliente",
        type: "GET",
        success: function (result) {
            if (result.ok) {
                for (let i = 0; i < result.return.length; i++) {
                    Clientes.push(result.return[i]);
                }

                var html = "<option value='0'>Seleccione...</option>";
                $("#clienteB").append(html);
                select = document.getElementById("clienteB");
                for (let i = 0; i < result.return.length; i++) {
                    var option = document.createElement('option');
                    option.text = result.return[i].nombre;
                    option.value = result.return[i].idCliente;
                    select.add(option);
                }

            } else {
                console.log(result.error);
            }
        },
        error: function (error) {
            console.log(error);
        },
    });

    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Transportista/ObtenerTransportistas",
        type: "GET",
        success: function (result) {
            
            // console.log(select.value);
        },
        error: function (error) {
            swal(result.error);
        }
    });

    // PARAMETROS DE BUSQUEDA
    $("#clienteB").change(function () {
        let id = $("#clienteB").val();

        if (id != 0) {
            $.ajax({
                url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerDetallesRemitos",
                type: "GET",

                success: function (result) {
                    if (result.ok) {
                        $("#cuerpoTablaEntregas").hide();
                        $("#cuerpoTablaEntregasP").empty();
                        $("#cuerpoTablaEntregasP").show();
                        $("#fechaB").val("");
                        $("#nroEntrega").val(0);
                        $("#transportistaB").val(0);
                        swal("Datos traidos con exito");

                        for (let index = 0; index < result.return.length; index++) {
                            if (result.return[index].idEstado == 3 && result.return[index].idCliente == id) {

                                let html = "<tr>";
                                html += "<td>" + result.return[index].idRemito + "</td>";
                                //html += "<td>" + datos[index].idEstadoNavigation.descripcion + "</td>";
                                html += "<td>" + roundDate(result.return[index].fechaCompra) + "</td>";
                                html += "<td>" + result.return[index].idHojaRutaNavigation.idTransportistaNavigation.nombre + "</td>";

                                let nombre = "";

                                for (let i = 0; i < Clientes.length; i++) {
                                    if (result.return[index].idCliente == Clientes[i].idCliente)
                                        nombre = Clientes[i].nombre;
                                }

                                html += "<td>" + nombre + "</td>";

                                html += "<td>" +
                                    "<button type='button' class='btn' id='eliminar' onclick='EliminarEntrega(" + result.return[index].idRemito + ")'>" +
                                    "<i class='bi bi-trash-fill'></i> </button>";

                                html += "&nbsp; <button type='button' class='btn' id='modificar' onclick='ModificarEntrega(" + result.return[index].idRemito + ")' data-toggle='modal' data-target='#ModificarEntrega' data-placement='bottom'>" +
                                    "<i class='bi bi-pencil-fill'></i> </button> "

                                html += "</td>";
                                html += "</tr>"

                                $("#cuerpoTablaEntregasP").append(html);
                            }
                        }

                    } else {
                        console.log(result.error);
                    }
                },
                error: function (error) {
                    swal("Problemas en el servidor");
                },
            })
        }
        else {
            $("#cuerpoTablaEntregas").show();
            $("#cuerpoTablaEntregasP").hide();
        }
    });

    $("#transportistaB").change(function () {
        let id = $("#transportistaB").val();

        if (id != 0) {
            $.ajax({
                url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerDetallesRemitos",
                type: "GET",

                success: function (result) {
                    if (result.ok) {
                        $("#cuerpoTablaEntregas").hide();
                        $("#cuerpoTablaEntregasP").empty();
                        $("#cuerpoTablaEntregasP").show();
                        $("#fechaB").val("");
                        $("#nroEntrega").val(0);
                        $("#clienteB").val(0);
                        swal("Datos traidos con exito");

                        for (let index = 0; index < result.return.length; index++) {
                            if (result.return[index].idEstado == 3 && result.return[index].idHojaRutaNavigation.idTransportista == id) {

                                let html = "<tr>";
                                html += "<td>" + result.return[index].idRemito + "</td>";
                                //html += "<td>" + datos[index].idEstadoNavigation.descripcion + "</td>";
                                html += "<td>" + roundDate(result.return[index].fechaCompra) + "</td>";
                                html += "<td>" + result.return[index].idHojaRutaNavigation.idTransportistaNavigation.nombre + "</td>";

                                let nombre = "";

                                for (let i = 0; i < Clientes.length; i++) {
                                    if (result.return[index].idCliente == Clientes[i].idCliente)
                                        nombre = Clientes[i].nombre;
                                }

                                html += "<td>" + nombre + "</td>";

                                html += "<td>" +
                                    "<button type='button' class='btn' id='eliminar' onclick='EliminarEntrega(" + result.return[index].idRemito + ")'>" +
                                    "<i class='bi bi-trash-fill'></i> </button>";

                                html += "&nbsp; <button type='button' class='btn' id='modificar' onclick='ModificarEntrega(" + result.return[index].idRemito + ")' data-toggle='modal' data-target='#ModificarEntrega' data-placement='bottom'>" +
                                    "<i class='bi bi-pencil-fill'></i> </button> "

                                html += "</td>";
                                html += "</tr>"

                                $("#cuerpoTablaEntregasP").append(html);
                            }
                        }

                    } else {
                        console.log(result.error);
                    }
                },
                error: function (error) {
                    swal("Problemas en el servidor");
                },
            })
        }
        else {
            $("#cuerpoTablaEntregas").show();
            $("#cuerpoTablaEntregasP").hide();
        }
    });

    $("#nroEntrega").change(function () {
        let id = $("#nroEntrega").val();

        if (id != 0) {
            $.ajax({
                url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerDetallesRemitos",
                type: "GET",

                success: function (result) {
                    if (result.ok) {
                        $("#cuerpoTablaEntregas").hide();
                        $("#cuerpoTablaEntregasP").empty();
                        $("#cuerpoTablaEntregasP").show();
                        $("#fechaB").val("");
                        $("#transportistaB").val(0);
                        $("#clienteB").val(0);
                        swal("Datos traidos con exito");

                        for (let index = 0; index < result.return.length; index++) {
                            if (result.return[index].idEstado == 3 && result.return[index].idRemito == id) {

                                let html = "<tr>";
                                html += "<td>" + result.return[index].idRemito + "</td>";
                                //html += "<td>" + datos[index].idEstadoNavigation.descripcion + "</td>";
                                html += "<td>" + roundDate(result.return[index].fechaCompra) + "</td>";
                                html += "<td>" + result.return[index].idHojaRutaNavigation.idTransportistaNavigation.nombre + "</td>";

                                let nombre = "";

                                for (let i = 0; i < Clientes.length; i++) {
                                    if (result.return[index].idCliente == Clientes[i].idCliente)
                                        nombre = Clientes[i].nombre;
                                }

                                html += "<td>" + nombre + "</td>";

                                html += "<td>" +
                                    "<button type='button' class='btn' id='eliminar' onclick='EliminarEntrega(" + result.return[index].idRemito + ")'>" +
                                    "<i class='bi bi-trash-fill'></i> </button>";

                                html += "&nbsp; <button type='button' class='btn' id='modificar' onclick='ModificarEntrega(" + result.return[index].idRemito + ")' data-toggle='modal' data-target='#ModificarEntrega' data-placement='bottom'>" +
                                    "<i class='bi bi-pencil-fill'></i> </button> "

                                html += "</td>";
                                html += "</tr>"

                                $("#cuerpoTablaEntregasP").append(html);
                            }
                        }

                    } else {
                        console.log(result.error);
                    }
                },
                error: function (error) {
                    swal("Problemas en el servidor");
                },
            })
        }
        else {
            $("#cuerpoTablaEntregas").show();
            $("#cuerpoTablaEntregasP").hide();
        }
    });

    function roundDatePlus(timeStamp) {
        var yyyy = new Date(timeStamp).getFullYear().toString();
        var mm = new Date(timeStamp).getMonth() + 1;
        var dd = new Date(timeStamp).getDate() + 1;
        return dd + "/" + mm + "/" + yyyy;
    }

    $("#fechaB").change(function () {
        let fecha = $("#fechaB").val();

        if (fecha != "") {
            $.ajax({
                url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerDetallesRemitos",
                type: "GET",

                success: function (result) {
                    if (result.ok) {
                        $("#cuerpoTablaEntregas").hide();
                        $("#cuerpoTablaEntregasP").empty();
                        $("#cuerpoTablaEntregasP").show();
                        $("#nroEntrega").val(0);
                        $("#transportistaB").val(0);
                        $("#clienteB").val(0);
                        swal("Datos traidos con exito");

                        for (let index = 0; index < result.return.length; index++) {
                            if (result.return[index].idEstado == 3 && roundDate(result.return[index].fechaCompra) == roundDatePlus(fecha)) {

                                let html = "<tr>";
                                html += "<td>" + result.return[index].idRemito + "</td>";
                                //html += "<td>" + datos[index].idEstadoNavigation.descripcion + "</td>";
                                html += "<td>" + roundDate(result.return[index].fechaCompra) + "</td>";
                                html += "<td>" + result.return[index].idHojaRutaNavigation.idTransportistaNavigation.nombre + "</td>";

                                let nombre = "";

                                for (let i = 0; i < Clientes.length; i++) {
                                    if (result.return[index].idCliente == Clientes[i].idCliente)
                                        nombre = Clientes[i].nombre;
                                }

                                html += "<td>" + nombre + "</td>";

                                html += "<td>" +
                                    "<button type='button' class='btn' id='eliminar' onclick='EliminarEntrega(" + result.return[index].idRemito + ")'>" +
                                    "<i class='bi bi-trash-fill'></i> </button>";

                                html += "&nbsp; <button type='button' class='btn' id='modificar' onclick='ModificarEntrega(" + result.return[index].idRemito + ")' data-toggle='modal' data-target='#ModificarEntrega' data-placement='bottom'>" +
                                    "<i class='bi bi-pencil-fill'></i> </button> "

                                html += "</td>";
                                html += "</tr>"

                                $("#cuerpoTablaEntregasP").append(html);
                            }
                        }

                    } else {
                        swal(result.error);
                    }
                },
                error: function (error) {
                    swal("Problemas en el servidor");
                },
            })
        }
        else {
            $("#cuerpoTablaEntregas").show();
            $("#cuerpoTablaEntregasP").hide();
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
                clienteEntregaId.value = result.return.cliente.idCliente;
                clienteEntrega.value = result.return.cliente.nombre;
                direccionEntrega.value = result.return.cliente.direccion;
                crearTablaDetalles(result.return.productosXRemitos);
                // switch (result.return.estado.idEstado) {
                //     case 1:
                //         $('#estadoEntrega').val(1);
                //         break;
                //     case 2:
                //         $('#estadoEntrega').val(2);
                //         break;
                //     case 3:
                //         $('#estadoEntrega').val(3);
                //         break;
                //     case 4:
                //         $('#estadoEntrega').val(4);
                //         break;

                //     default: $('#estadoEntrega').val(5);
                //         break;
                // }
            } else {
                console.log(result.error);
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
    $("#cuerpoTablaEntregasP").hide();
    $("#cuerpoTablaEntregas tr").remove();
    //codigo, estado, fecha, transpotista
    for (let index = 0; index < datos.length; index++) {
        if (datos[index].idEstado == 3) {

            let html = "<tr>";
            html += "<td>" + datos[index].idRemito + "</td>";
            //html += "<td>" + datos[index].idEstadoNavigation.descripcion + "</td>";
            html += "<td>" + roundDate(datos[index].fechaCompra) + "</td>";
            html += "<td>" + datos[index].idHojaRutaNavigation.idTransportistaNavigation.nombre + "</td>";

            let nombre = "";

            for (let i = 0; i < Clientes.length; i++) {
                if (datos[index].idCliente == Clientes[i].idCliente)
                    nombre = Clientes[i].nombre;
            }

            html += "<td>" + nombre + "</td>";

            html += "<td>" +
                "<button type='button' class='btn' id='eliminar' onclick='EliminarEntrega(" + datos[index].idRemito + ")'>" +
                "<i class='bi bi-trash-fill'></i> </button>";

            html += "&nbsp; <button type='button' class='btn' id='modificar' onclick='ModificarEntrega(" + datos[index].idRemito + ")' data-toggle='modal' data-target='#ModificarEntrega' data-placement='bottom'>" +
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

function sumarDias(fecha, dias) {
    fecha.setDate(fecha.getDate() + dias);
    return fecha;
}

function CargarEntrega() {
    let idRemito = $("#idRemito").val();
    let horaEntrega = $("#horaEntrega").val();
    var firma = null;
    let observaciones = $("#observaciones").val();
    let idEstado = $("#estadoEntrega").val();
    CargarDetalleEntrega(idRemito, horaEntrega, firma, observaciones);
    ActualizarEstadoEntregado(idRemito, idEstado);
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
                swal("Registro Exitoso")
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
                swal("Entrega modificada correctamente");
            } else {
                swal("Problema Server");
            }
        },
        error: function (error) {
            swal("Error");
        },
    })

}


function EliminarEntrega(id) {


    $.ajax({
        url: "https://localhost:5001/Entregas/DeleteEntrega?IdDetalle=" + id,//falta heroku
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

function ActualizarEstadoEntregado(id, idEstado) {
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/ObtenerRemito?id=" + id,
        type: "GET",
        success: function (result) {
            if (result.ok) {
                actualizarEstado(id, result.return.fechaCompra, result.return.cliente.idCliente, result.return.hojaRuta.idHojaRuta, idEstado);
            } else {
                swal(result.error);
            }
        },
        error: function (error) {
            console.log(error);
        },
    });
}

function actualizarEstado(id, fecha, idCliente, idHojaRuta, idEstado) {
    let fechaR = fecha;
    const hoy = new Date();

    if (idEstado == 4)
        fechaR = sumarDias(hoy, 1);

    comando = {
        "idRemito": id,
        "fechaCompra": fechaR,
        "horaEntregaPreferido": "16:00",
        "idEstado": idEstado,
        "idCliente": idCliente,
        "idHojaRuta": idHojaRuta
    }

    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Remito/Actualizar",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(comando),
        type: "PUT",
        success: function (result) {
            if (result.ok) {
                //swal("Estado modificado exitosamente");
                location.reload();
            }
            else swal(result.error);
        },
        error: function (error) {
            swal("Error" + error);
        }
    });
}
function habilitarBoton() {
    $("#habilitarDireccion").click(function () {
        $("#direccionEntrega").prop("readonly", false);
        $("#modificarDireccion").prop("disabled", false);
    });
}
function deshabilitarBoton() {
    $("#habilitarDireccion").click(function () {
        $("#direccionEntrega").prop("readonly", true);
        $("#modificarDireccion").prop("disabled", true);
    });
}
function botonModificarDireccion() {
    $("#modificarDireccion").click(function () {

        deshabilitarBoton();
        let id = $("#clienteEntregaId").val();
        let direccion = $("#direccionEntrega").val();

       
        modificarDireccione(id, direccion);


    });
}

function modificarDireccione(id, direccion) {
    comando = {
        "idCliente": parseInt(id),
        "direccion": direccion,
    };
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Direccion/ModificarDireccion",//Heroku
        type: "PUT",
        dataType: 'JSON',
        contentType: 'application/json',
        data: JSON.stringify(comando),
        success: function (result) {
            if (result.ok) {
                swal("Modificacion exitosa");
            } else {
                swal("Problema Server");
            }
        },
        error: function (error) {
            swal("Problemas en el servidor");
        },
    })
}



// https://vast-brook-85314.herokuapp.com/swagger/index.html