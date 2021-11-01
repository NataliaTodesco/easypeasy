using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyPeasy.Comandos
{
    public class ComandoVehiculo
    {
        public int IdVehiculo { get; set; }

        [Required(ErrorMessage = "Ingrese la Patente")]
        public string Patente { get; set; }

        public string Modelo { get; set; }

        public string Color { get; set; }
       
    }
}
