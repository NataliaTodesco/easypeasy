using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Estado
    {
        public Estado()
        {
            Remitos = new HashSet<Remito>();
        }

        public int IdEstado { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Remito> Remitos { get; set; }
    }
}
