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
    
    public partial class Tbl_OrdenTmp
    {
        public int id_OrdenTmp { get; set; }
        public Nullable<int> id_Articulo { get; set; }
        public string user { get; set; }
        public Nullable<int> cantidad { get; set; }
    
        public virtual Tbl_Articulo Tbl_Articulo { get; set; }
    }
}