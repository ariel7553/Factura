using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
    public class ProductoModel
    {
        public int Num { get; set; }
        public int Producto_Id { get; set; }
        public string Producto1 { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Cant { get; set; }
        public decimal Total { get; set; }
        public int Estado { get; set; }


    }
}