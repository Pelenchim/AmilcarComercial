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
    
    public partial class Tbl_Categorias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Categorias()
        {
            this.Tbl_Articulo = new HashSet<Tbl_Articulo>();
            this.Tbl_Categorias1 = new HashSet<Tbl_Categorias>();
            this.Tbl_Descripciones = new HashSet<Tbl_Descripciones>();
        }
    
        public int id_categoria { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> id_CatPadre { get; set; }
        public bool estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Articulo> Tbl_Articulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Categorias> Tbl_Categorias1 { get; set; }
        public virtual Tbl_Categorias Tbl_Categorias2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Descripciones> Tbl_Descripciones { get; set; }
    }
}
