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
    
    public partial class Tbl_Proveedor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Proveedor()
        {
            this.Tbl_Compra = new HashSet<Tbl_Compra>();
        }
    
        public int id_proveedor { get; set; }
        public string razon_social { get; set; }
        public string telefono { get; set; }
        public string Ruc { get; set; }
        public Nullable<bool> Estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Compra> Tbl_Compra { get; set; }
    }
}
