using System.Dynamic;
using System;
using System.Collections.Generic;

namespace EasyPeasy.Models.DTO
{
    public class DTOHojaRuta
    {

        public int Id { get; set; }

        public int? IdZona { get; set; }

        public DateTime? Fecha { get; set; }

        public int? IdVehiculo { get; set; }
        public HRVehiculo Vehiculo {get; set;}
        public int? IdTransportista {get;set;}
        public HRTransportista Transportista { get; set; }

        public List<HRemito> Remitos{get;set;}

        public List<string> Direcciones{get;set;}
    }

    public class HRemito{

        public int IdRemito { get; set; }
        public DateTime? FechaCompra { get; set; }
        public TimeSpan? HoraEntregaPreferido { get; set; }
        public int? Estado { get; set; }
        public HCliente Cliente { get; set; }
        
    }

        public class Rutas{
       public List<DTOHojaRuta> HojaRutas{get;set;}
    }
    public class HCliente{
         public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }

    public class HRTransportista {
        public int IdTransportista { get; set; }
        public long Legajo { get; set; }
        public string Nombre { get; set; }
    }

    public class HRVehiculo{
        public int IdVehiculo { get; set; }
        public string Patente { get; set; }
        public string Modelo { get; set; }
    }
 }