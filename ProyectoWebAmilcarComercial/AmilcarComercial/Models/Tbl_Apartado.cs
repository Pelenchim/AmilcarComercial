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
    
    public partial class Tbl_Apartado
    {
        public int id_apartado { get; set; }
        public System.DateTime fecha_plazo { get; set; }
        public double importe { get; set; }
        public int id_orden { get; set; }
    
        public virtual Tbl_Orden Tbl_Orden { get; set; }
        public virtual Tbl_Pago_Apartado Tbl_Pago_Apartado { get; set; }
    }
}
