//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmilcarComercial.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_garantia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_garantia()
        {
            this.Tbl_Devolucion = new HashSet<Tbl_Devolucion>();
        }
    
        public int id_garantia { get; set; }
        public string cod_garantia { get; set; }
        public string plazo_garantia { get; set; }
        public string estado { get; set; }
        public int id_detalle_orden { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Devolucion> Tbl_Devolucion { get; set; }
    }
}