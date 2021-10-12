using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Barrio
    {
        public Barrio()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdBarrio { get; set; }
        public string Descripcion { get; set; }
        public int IdZona { get; set; }

        public virtual Zona IdZonaNavigation { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
