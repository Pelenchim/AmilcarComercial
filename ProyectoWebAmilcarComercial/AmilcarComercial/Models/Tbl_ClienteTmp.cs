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
    
    public partial class Tbl_ClienteTmp
    {
        public int id_clienteTmp { get; set; }
        public string nombre_cliente { get; set; }
        public string apellidos_cliente { get; set; }
        public string direccion { get; set; }
        public int departamento { get; set; }
        public Nullable<int> telefono { get; set; }
        public string cedula { get; set; }
        public string user { get; set; }
        public bool nuevo { get; set; }
        public Nullable<int> id_cliente { get; set; }
    
        public virtual Tbl_Departamentos Tbl_Departamentos { get; set; }
    }
}
