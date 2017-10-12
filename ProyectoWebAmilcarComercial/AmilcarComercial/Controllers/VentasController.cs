using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    public class VentasController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Ventas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Nueva()
        {
            return View();
        }

        [Route("obtener/articulos")]
        [HttpGet]
        public JsonResult ObtenerArticulos()
        {
            var Articulos = (from p in db.Tbl_Articulo.Where(m => m.estado == true).OrderByDescending(m => m.id_articulo)
                             select new
                             {
                                 ID = p.id_articulo,
                                 Codigo = p.codigo_articulo,
                                 Nombre = p.nombre_articulo,
                                 Imagen = p.imagen
                             }).ToList();

            return Json(new { data = Articulos }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/clientes")]
        [HttpGet]
        public JsonResult ObtenerClientes()
        {
            var Clientes = (from p in db.Tbl_Clientes.Where(m => m.estado == true).OrderByDescending(m => m.id_cliente)
                            select new
                            {
                                ID = p.id_cliente,
                                Nombre = p.nombre_cliente,
                                Apellido = p.apellidos_cliente,
                                Departamento = p.departamento,
                                Telefono = p.telefono
                            }).ToList();

            return Json(new { data = Clientes }, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/clienteTmp")]
        [HttpPost]
        public JsonResult AgregarCliente(Tbl_ClienteTmp cliente)
        {
            cliente.user = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Tbl_ClienteTmp.Add(cliente);
                db.SaveChanges();
            }

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/clienteTmp")]
        [HttpGet]
        public JsonResult MostrarClienteTmp(Tbl_ClienteTmp cliente)
        {
            var data = (from c in db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).OrderByDescending(m => m.id_clienteTmp)
                         select new
                         {
                             Nombre = c.nombre_cliente,
                             Apellido = c.apellidos_cliente,
                             Cedula = c.cedula,
                             Telefono = c.telefono,
                             Direccion = c.direccion,
                             Departamento = c.Tbl_Departamentos.Nombre
                         }).First();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/clienteExistente/{id}")]
        [HttpGet]
        public JsonResult AgregarClienteExistente(int id)
        {
            var user = User.Identity.Name;
            Tbl_Clientes cliente = db.Tbl_Clientes.Find(id);
            Tbl_ClienteTmp clienteTmp = new Tbl_ClienteTmp();

            clienteTmp.user = user;
            clienteTmp.nombre_cliente = cliente.nombre_cliente;
            clienteTmp.apellidos_cliente = cliente.apellidos_cliente;
            clienteTmp.cedula = cliente.cedula;
            clienteTmp.telefono = cliente.telefono;
            clienteTmp.departamento = cliente.departamento;
            clienteTmp.direccion = cliente.direccion;

            db.Tbl_ClienteTmp.Add(clienteTmp);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/producto/{id}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id)
        {


            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }
    }
}