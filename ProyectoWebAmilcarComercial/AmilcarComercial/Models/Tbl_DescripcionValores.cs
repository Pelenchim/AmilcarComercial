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
    
    public partial class Tbl_DescripcionValores
    {
        public int id_descripcionValor { get; set; }
        public Nullable<int> id_descripcion { get; set; }
        public string valor { get; set; }
        public Nullable<int> id_articulo { get; set; }
    
        public virtual Tbl_Articulo Tbl_Articulo { get; set; }
        public virtual Tbl_Descripciones Tbl_Descripciones { get; set; }
    }
}
