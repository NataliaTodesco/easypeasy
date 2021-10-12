using System;

namespace api.Resultados
{
     public class ResultadoApi{
        public Boolean Ok { get; set; }
        public string Error { get; set; }
        public int codigoError { get; set; }
        public string infoAdicional { get; set; }
        public Object Return { get; set; }
    }
}