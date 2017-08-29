using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmilcarComercial.ViewsModels
{
    public class UsuariosViewModel
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Foto { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
}