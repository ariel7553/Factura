//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Facturacion.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public partial class Factura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Factura()
        {
            this.Detalle = new HashSet<Detalle>();
        }
    
        public int Id_Fact { get; set; }
        public System.DateTime Fecha { get; set; }
        public int Id_Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Detalle> Detalle { get; set; }
    }
}
