using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyPeasy.Comandos
{
    public class ComandoHojaRuta
    {
        public int IdHojaRuta { get; set; }
        
        //propiedad para ordenar remitos, no c inserta en bd
        //===================================

        [Required(ErrorMessage = "Ingrese un ID de zona.")]
        public int? idZona {get;set;}
        //===================================

        [Required(ErrorMessage = "Ingrese una fecha.")]
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }
        //===================================

        [Required(ErrorMessage = "Ingrese un ID de vehiculo.")]
        public int? IdVehiculo { get; set; }
        //===================================

        [Required(ErrorMessage = "Ingrese un ID de transportista.")]
        public int? IdTransportista { get; set; }
        //===================================

    }
}