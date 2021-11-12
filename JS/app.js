  function mostrarMensaje() {
    swal("Información del Sistema","Trabajo práctico integrador realizado para la materia Metodología de Sistemas I, de la Tecnicatura Universitaria en Programación, ciclo lectivo 2021. Que tiene como objetivo  llevar adelante un proyecto de desarrollo de software a medida, aplicando los conceptos teóricos y prácticos aprendidos durante el desarrollo de la asignatura");
  }

  function go(){
    // Validación del Form
    var forms = document.getElementsByClassName('needs-validation');
      
      var validation = Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('click', function (event) {
          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
          }
          form.classList.add('was-validated');
        }, false);
      });

    // Login
    var rol = document.getElementById('rol');
    var idRol = rol.selectedIndex+1;

    if ($("#contraseña").val()=='Admin123456' && $("#email").val() =='administrador@gmail.com' && idRol == 1){ 
      window.location.replace("./menu.html");
    } 
    
    else if ($("#contraseña").val()=='Gerente123456' && $("#email").val() =='gerente@gmail.com' && idRol == 2){
      window.location.replace("./menuReportes.html");
    } 
    
    else if ($("#contraseña").val()=='Transp123456' && $("#email").val() =='transportista@gmail.com' && idRol == 4){
      window.location.replace("./IndexHojaDeRuta.html");
    } 
    
    else if ($("#contraseña").val()=='Emple123456' && $("#email").val() =='empleado@gmail.com' && idRol == 3){
      window.location.replace("./remitos.html");
    }
    else if ($("#contraseña").val()=='Resp123456' && $("#email").val() =='ResponsableRutas@gmail.com' && idRol == 5){
      window.location.replace("./remitos.html");
    }
    else{ 
      swal("Error de Validacion","Porfavor ingrese email, rol y contraseña correctos","error"); 
    } 
  } 

  

