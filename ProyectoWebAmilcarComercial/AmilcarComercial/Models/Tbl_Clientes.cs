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
    
    public partial class Tbl_Clientes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Clientes()
        {
            this.Tbl_Orden = new HashSet<Tbl_Orden>();
        }
    
        public int id_cliente { get; set; }
        public string nombre_cliente { get; set; }
        public string apellidos_cliente { get; set; }
        public string direccion { get; set; }
        public int departamento { get; set; }
        public int telefono { get; set; }
        public string cedula { get; set; }
        public Nullable<bool> estado { get; set; }
    
        public virtual Tbl_Departamentos Tbl_Departamentos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Orden> Tbl_Orden { get; set; }
    }
}
