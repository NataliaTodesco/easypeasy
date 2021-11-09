using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Disponibilidad{
        public int Id { get; set; }
        public string Nombre { get; set; }
    
     public virtual ICollection<Transportista> Transportistas { get; set; }
    }
}