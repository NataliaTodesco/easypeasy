using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Transportista
    {
        public Transportista()
        {
            HojaRuta = new HashSet<HojaRuta>();
        }

        public int IdTransportista { get; set; }
        public long Legajo { get; set; }
        public string Nombre { get; set; }
        public int? idDisponibilidad { get; set; }

        public virtual ICollection<HojaRuta> HojaRuta { get; set; }
        public virtual Disponibilidad IdDisponibilidadNavigation {get;set;}
    }
}
