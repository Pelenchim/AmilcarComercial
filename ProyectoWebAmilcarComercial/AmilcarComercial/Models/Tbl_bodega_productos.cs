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
    
    public partial class Tbl_bodega_productos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_bodega_productos()
        {
            this.Bodega_Detalle = new HashSet<Bodega_Detalle>();
        }
    
        public int id_bodega_productos1 { get; set; }
        public int id_bodega { get; set; }
        public int id_articulo { get; set; }
        public int stock { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bodega_Detalle> Bodega_Detalle { get; set; }
        public virtual Tbl_Articulo Tbl_Articulo { get; set; }
        public virtual Tbl_bodega Tbl_bodega { get; set; }
    }
}