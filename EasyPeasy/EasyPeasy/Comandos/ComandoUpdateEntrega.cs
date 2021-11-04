using System;

namespace EasyPeasy.Comandos
{
    public class ComandoUpdateEntrega
    {
        public int IdDetalle { get; set; }
        public TimeSpan? HoraEntrega { get; set; }

        public byte[] Firma { get; set; }
        public string Observaciones { get; set; }

    }
}