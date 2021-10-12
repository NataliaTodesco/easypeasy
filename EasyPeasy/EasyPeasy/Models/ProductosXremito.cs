using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class ProductosXremito
    {
        public int IdProducto { get; set; }
        public int IdRemito { get; set; }
        public int? Cantidad { get; set; }

        public virtual Producto IdProductoNavigation { get; set; }
        public virtual Remito IdRemitoNavigation { get; set; }
    }
}
