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
    
    public partial class Tbl_Configuracion
    {
        public int id_Configuracion { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public Nullable<double> monto { get; set; }
        public Nullable<int> id_sucursal { get; set; }
        public Nullable<double> tasacambio { get; set; }
    }
}
