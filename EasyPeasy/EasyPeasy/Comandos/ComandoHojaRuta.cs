using System;

namespace EasyPeasy.Comandos
{
    public class ComandoHojaRuta
    {
        public int IdHojaRuta { get; set; }
        
        //propiedad para ordenar remitos, no c inserta en bd
        public int? idZona {get;set;}
        public DateTime? Fecha { get; set; }
        public int? IdVehiculo { get; set; }
        public int? IdTransportista { get; set; }
    }
}