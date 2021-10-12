using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Vehiculo
    {
        public Vehiculo()
        {
            HojaRuta = new HashSet<HojaRuta>();
        }

        public int IdVehiculo { get; set; }
        public string Patente { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }

        public virtual ICollection<HojaRuta> HojaRuta { get; set; }
    }
}
