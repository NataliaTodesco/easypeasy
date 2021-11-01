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

    IdVehiculo = document.getElementById("IdVehiculo");
    Patente = document.getElementById('Patente');
    Modelo = document.getElementById('Modelo');
    Color = document.getElementById('Color');

    IdVehiculo = document.getElementById("idVehiculo");
    Patente = document.getElementById('patente');
    Modelo = document.getElementById('modelo');
    Color = document.getElementById('color');

    //Obtener Remitos
    $.ajax({
        url: "https://vast-brook-85314.herokuapp.com/Vehiculo/ObtenerVehiculos",
        type: "GET",

        success: function (result) {
            if (result.ok) {
                crearTabla(result.return);
                //cargarComboId(result.return);
            } else {
                swal(result.error);
            }
        },
        error: function (error) {
            console.log(); (error);
        },
    });



    function crearTabla(datos) {
        $("#cuerpoTablaVehiculos tr").remove();
        for (let index = 0; index < datos.length; index++) {
            let html = "<tr>";
            html += "<td>" + datos[index].idVehiculo + "</td>";
            html += "<td>" + datos[index].patente + "</td>";
            html += "<td>" + datos[index].modelo + "</td>";
            html += "<td>" + datos[index].color + "</td>";
            
            $("#cuerpoTablaVehiculos").append(html);
        }
    }
}