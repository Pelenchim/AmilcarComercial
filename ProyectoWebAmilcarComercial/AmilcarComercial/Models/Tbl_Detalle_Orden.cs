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
    
    public partial class Tbl_Detalle_Orden
    {
        public int id_detalle_orden { get; set; }
        public int id_articulo { get; set; }
        public int id_orden { get; set; }
        public int cantidad { get; set; }
        public double precio_venta { get; set; }
        public Nullable<double> descuento { get; set; }
        public int id_kardex { get; set; }
    
        public virtual Tbl_Articulo Tbl_Articulo { get; set; }
        public virtual Tbl_Kardex Tbl_Kardex { get; set; }
        public virtual Tbl_Orden Tbl_Orden { get; set; }
    }
}
