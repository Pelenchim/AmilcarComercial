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
    
    public partial class Tbl_DevolucionTmp
    {
        public int id_devolucion { get; set; }
        public string tipo { get; set; }
        public Nullable<int> id_compra { get; set; }
        public string user { get; set; }
        public Nullable<int> id_venta { get; set; }
    
        public virtual Tbl_Compra Tbl_Compra { get; set; }
        public virtual Tbl_Orden Tbl_Orden { get; set; }
    }
}
