using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Remito
    {
        public Remito()
        {
            DetalleEntregas = new HashSet<DetalleEntrega>();
            ProductosXremitos = new HashSet<ProductosXremito>();
        }

        public int IdRemito { get; set; }
        public DateTime? FechaCompra { get; set; }
        public TimeSpan? HoraEntregaPreferido { get; set; }
        public int? IdEstado { get; set; }
        public int? IdCliente { get; set; }
        public int? IdHojaRuta { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Estado IdEstadoNavigation { get; set; }
        public virtual HojaRuta IdHojaRutaNavigation { get; set; }
        public virtual ICollection<DetalleEntrega> DetalleEntregas { get; set; }
        public virtual ICollection<ProductosXremito> ProductosXremitos { get; set; }
    }
}
