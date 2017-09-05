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
    
    public partial class Tbl_Articulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Articulo()
        {
            this.SalidaDetalle = new HashSet<SalidaDetalle>();
            this.Tbl_bodega_productos = new HashSet<Tbl_bodega_productos>();
            this.Tbl_Detalle_Orden = new HashSet<Tbl_Detalle_Orden>();
            this.Tbl_Kardex = new HashSet<Tbl_Kardex>();
        }
    
        public int id_articulo { get; set; }
        public string codigo_articulo { get; set; }
        public string nombre_articulo { get; set; }
        public string descripcion_articulo { get; set; }
        public byte[] imagen { get; set; }
        public int id_categoria { get; set; }
        public int credito_articulo { get; set; }
        public int id_Marca { get; set; }
        public string Garantia { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalidaDetalle> SalidaDetalle { get; set; }
        public virtual Tbl_Categorias Tbl_Categorias { get; set; }
        public virtual Tbl_Marca Tbl_Marca { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_bodega_productos> Tbl_bodega_productos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Detalle_Orden> Tbl_Detalle_Orden { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Kardex> Tbl_Kardex { get; set; }
    }
}
