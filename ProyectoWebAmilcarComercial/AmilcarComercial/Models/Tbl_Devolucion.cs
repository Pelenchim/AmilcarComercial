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
    
    public partial class Tbl_Devolucion
    {
        public double id_devolucion { get; set; }
        public System.DateTime fecha_devolucion { get; set; }
        public string usuario { get; set; }
        public int id_garantia { get; set; }
        public int id_orden { get; set; }
        public int id_kardex { get; set; }
    
        public virtual Tbl_garantia Tbl_garantia { get; set; }
    }
}
