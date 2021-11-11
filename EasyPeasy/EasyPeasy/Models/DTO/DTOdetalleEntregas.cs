using System.Net.NetworkInformation;
using System.Dynamic;
using System;
using System.Collections.Generic;

namespace EasyPeasy.Models.DTO
{
    public class DTOdetalleEntregas
    {
        public int Id { get; set; }
        public int? IdRemito { get; set; }
        public TimeSpan? HoraEntrega { get; set; }
        public byte[] Firma { get; set; }
        public string Observaciones { get; set; }

        public List<DRemito> Remitos { get; set; }
    }
    public class DRemito{
        public int IdRemito { get; set; }
        public DateTime? FechaCompra { get; set; }
        public int? IdHojaRuta { get; set; }
        public int? IdEstado { get; set; }
        public DHojaRuta HojaRuta{get;set;}
    }
     public class DHojaRuta { 
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdTransportista { get; set; }
     }
   public class Detalles
   {
        public List<DTOdetalleEntregas> DTOdetalleEntregas {get;set;} 
   }
}