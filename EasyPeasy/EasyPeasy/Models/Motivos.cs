using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Motivos
    { 
        public int IdMotivo { get; set; }
        public string Descripcion { get; set; }
       // public virtual ICollection<DetalleEntrega> DetalleEntregas { get; set; }
    }
}