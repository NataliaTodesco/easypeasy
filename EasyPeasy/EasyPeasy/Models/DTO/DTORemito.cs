using System.Dynamic;
using System;
using System.Collections.Generic;

namespace EasyPeasy.Models.DTO
{
    public class DTORemito
    {

        public int IdRemito { get; set; }
        public DateTime? FechaCompra { get; set; }
        public TimeSpan? HoraEntregaPreferido { get; set; }
        public REstado Estado { get; set; }
        public RCliente Cliente { get; set; }
        public RHojaRuta HojaRuta { get; set; }
        public List<RProductoXRemito> ProductosXRemitos {get; set;}
    }


        public class RHojaRuta{
        public int? IdHojaRuta {get;set;}
        public DateTime? Fecha { get; set; }
        public int? IdVehiculo { get; set; }
        public int? IdTransportista { get; set; } 
    }
    public class RCliente{
         public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
    public class REstado{
        public int IdEstado { get; set; }
        public string Descripcion { get; set; }
    }
    public class RProductoXRemito{
        public RProducto Producto { get; set; }
        public int? Cantidad { get; set; }
    }
    public class RProducto{
        public int IdProducto {get;set;}
        public string Descripcion {get;set;}
    }
    public class ListadoRemitos{
        public List<DTORemito> Remitos{get;set;}
    }
     public class DTORemitoXProducto
    {
        
        public int IdRemito{get;set;}
        public int IdProducto{get;set;}
        public int Cantidad{get;set;}
        public RProducto Producto{get;set;}
        public DTORemito Remito{get;set;}
    }
    public class ListadoRemitosXProductos{
        public List<DTORemitoXProducto> RemitosXProductos{get;set;}
    }
}