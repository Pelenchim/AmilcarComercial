//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmilcarComercial.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_DevolucionCliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_DevolucionCliente()
        {
            this.Tbl_DetalleDevolucionCliente = new HashSet<Tbl_DetalleDevolucionCliente>();
        }
    
        public int id_devolucionCliente { get; set; }
        public System.DateTime fecha { get; set; }
        public int id_venta { get; set; }
        public string fact { get; set; }
        public string user { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DetalleDevolucionCliente> Tbl_DetalleDevolucionCliente { get; set; }
        public virtual Tbl_Orden Tbl_Orden { get; set; }
    }
}
