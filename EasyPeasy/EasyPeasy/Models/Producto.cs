using System;
using System.Collections.Generic;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class Producto
    {
        public Producto()
        {
            ProductosXremitos = new HashSet<ProductosXremito>();
        }

        public int IdProducto { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<ProductosXremito> ProductosXremitos { get; set; }
    }
}
