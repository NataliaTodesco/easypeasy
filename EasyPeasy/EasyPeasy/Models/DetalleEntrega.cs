using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class DetalleEntrega
    {
        public int IdDetalle { get; set; }
        public int? IdRemito { get; set; }
        public TimeSpan? HoraEntrega { get; set; }
        public byte[] Firma { get; set; }
        public string Observaciones { get; set; }
       // public int? IdMotivo { get; set; }

        public virtual Remito IdRemitoNavigation { get; set; }
       // public virtual Motivos IdMotivoNavigation {get;set;}
    }
}
