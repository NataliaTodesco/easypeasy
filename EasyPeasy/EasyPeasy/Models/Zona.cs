using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Zona
    {
        public Zona()
        {
            Barrios = new HashSet<Barrio>();
        }

        public int IdZona { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Barrio> Barrios { get; set; }
    }
}
