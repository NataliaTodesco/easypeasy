using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Remitos = new HashSet<Remito>();
        }

        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int IdBarrio { get; set; }

        public virtual Barrio IdBarrioNavigation { get; set; }
        public virtual ICollection<Remito> Remitos { get; set; }
    }
}
