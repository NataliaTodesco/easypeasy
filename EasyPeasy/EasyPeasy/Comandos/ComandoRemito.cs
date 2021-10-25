using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyPeasy.Comandos
{
    public class ComandoRemito
    {
        
        public int IdRemito { get; set; }
        //===================================

        [Required(ErrorMessage = "Ingrese una fecha.")]
        [DataType(DataType.Date)]
        public DateTime? FechaCompra { get; set; }
        //===================================

        [Required(ErrorMessage = "Ingrese un horario.")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraEntregaPreferido { get; set; }
        //===================================

        [Required(ErrorMessage = "Ingrese un ID de estado.")]
        public int? IdEstado { get; set; }
        //===================================

        [Required(ErrorMessage = "Ingrese un ID de cliente.")]
        public int? IdCliente { get; set; }
        //===================================

        [Required(ErrorMessage = "Ingrese un ID de Hoja de ruta.")]
        public int? IdHojaRuta { get; set; }
        //===================================

    }
}
