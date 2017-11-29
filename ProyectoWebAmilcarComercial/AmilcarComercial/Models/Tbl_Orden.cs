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
    
    public partial class Tbl_Orden
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Orden()
        {
            this.Tbl_Apartado = new HashSet<Tbl_Apartado>();
            this.Tbl_Credito = new HashSet<Tbl_Credito>();
            this.Tbl_Detalle_Orden = new HashSet<Tbl_Detalle_Orden>();
            this.Tbl_DevolucionTmp1 = new HashSet<Tbl_DevolucionTmp>();
            this.Tbl_DevolucionCliente = new HashSet<Tbl_DevolucionCliente>();
        }
    
        public int id_orden { get; set; }
        public Nullable<int> id_sucursal { get; set; }
        public string usuario { get; set; }
        public System.DateTime fecha_orden { get; set; }
        public double iva_orden { get; set; }
        public string tipo_orden { get; set; }
        public string fact_Orden { get; set; }
        public int id_cliente { get; set; }
        public string tipo_pago { get; set; }
        public Nullable<bool> estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Apartado> Tbl_Apartado { get; set; }
        public virtual Tbl_Clientes Tbl_Clientes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Credito> Tbl_Credito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Detalle_Orden> Tbl_Detalle_Orden { get; set; }
        public virtual Tbl_Sucursal Tbl_Sucursal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DevolucionTmp> Tbl_DevolucionTmp1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DevolucionCliente> Tbl_DevolucionCliente { get; set; }
    }
}
