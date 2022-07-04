using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
    public class FacturaModel
    {
        public int Num { get; set; }
        public int Id_Fact { get; set; }
        public System.DateTime Fecha { get; set; }
        public int Id_Cliente { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
    }
}