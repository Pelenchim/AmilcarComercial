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
    
    public partial class Tbl_Kardex
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Kardex()
        {
            this.SalidaDetalle = new HashSet<SalidaDetalle>();
            this.Tbl_Detalle_Compra = new HashSet<Tbl_Detalle_Compra>();
            this.Tbl_Detalle_Orden = new HashSet<Tbl_Detalle_Orden>();
        }
    
        public int id_Kardex { get; set; }
        public int id_articulo { get; set; }
        public System.DateTime fechaKardex { get; set; }
        public string num_factura { get; set; }
        public int Entrada { get; set; }
        public int salida { get; set; }
        public int saldo { get; set; }
        public double costoPromedio { get; set; }
        public string usuario { get; set; }
        public int id_sucursal { get; set; }
        public double ultimoCosto { get; set; }
        public string tipo { get; set; }
        public string observaciones { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalidaDetalle> SalidaDetalle { get; set; }
        public virtual Tbl_Articulo Tbl_Articulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Detalle_Compra> Tbl_Detalle_Compra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Detalle_Orden> Tbl_Detalle_Orden { get; set; }
        public virtual Tbl_Sucursal Tbl_Sucursal { get; set; }
    }
}
