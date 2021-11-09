using System;
using System.Collections.Generic;
using EasyPeasy.Models;

namespace EasyPeasy.Comandos
{
    public class ComandoDetalleEntrega
    {
        public int IdDetalle { get; set; }
        public int? IdRemito { get; set; }
        public TimeSpan HoraEntrega { get; set; }
        public byte[] Firma { get; set; }
        public string Observaciones { get; set; }
       // public int? IdMotivo { get; set; }
        //public int? IdEstado { get; set; }
    }
}