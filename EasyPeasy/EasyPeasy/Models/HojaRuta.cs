using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class HojaRuta
    {
        public HojaRuta()
        {
            Remitos = new HashSet<Remito>();
        }

        public int IdHojaRuta { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdVehiculo { get; set; }
        public int? IdTransportista { get; set; }

        public virtual Transportista IdTransportistaNavigation { get; set; }
        public virtual Vehiculo IdVehiculoNavigation { get; set; }
        public virtual ICollection<Remito> Remitos { get; set; }
    }
}
