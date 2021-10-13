using System;

namespace EasyPeasy.Comandos
{
    public class ComandoRemito
    {
        public int IdRemito { get; set; }
        public DateTime? FechaCompra { get; set; }
        public TimeSpan? HoraEntregaPreferido { get; set; }
        public int? IdEstado { get; set; }
        public int? IdCliente { get; set; }
        public int? IdHojaRuta { get; set; }
    }
}
