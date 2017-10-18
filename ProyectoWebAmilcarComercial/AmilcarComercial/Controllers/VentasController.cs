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
                                Departamento = p.Tbl_Departamentos.Nombre,
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
            if (db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).Count() != 0)
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
            else
            {
                var data = 0;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("agregar/clienteExistente/{id}")]
        [HttpGet]
        public JsonResult AgregarClienteExistente(int id)
        {
            var user = User.Identity.Name;
            Tbl_Clientes cliente = db.Tbl_Clientes.Find(id);
            Tbl_ClienteTmp clienteTmp = new Tbl_ClienteTmp();
            EliminarClientesTmp();

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

        //metodo que elimina los clientes existentes en la tabla temporal
        [Route("eliminar/clienteTmp")]
        public JsonResult EliminarClientesTmp()
        {
            var lista = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).ToList();

            if (lista != null)
            {
                db.Tbl_ClienteTmp.RemoveRange(lista);
                db.SaveChanges();
            }
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("editar/clienteTmp")]
        [HttpGet]
        public JsonResult EditarClienteTmp()
        {
            var cliente = (from c in db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name)
                           select new
                           {
                               ID = c.id_clienteTmp,
                               Nombre = c.nombre_cliente,
                               Apellido = c.apellidos_cliente,
                               Direccion = c.direccion,
                               Departamento = c.departamento,
                               Telefono = c.telefono,
                               Cedula = c.cedula
                           }).FirstOrDefault();
            return Json(new { data = cliente }, JsonRequestBehavior.AllowGet);
        }

        [Route("editar/clienteGuardarTmp")]
        [HttpPost]
        public JsonResult EditarGuardarClienteTmp(Tbl_ClienteTmp cliente)
        {
            var user = User.Identity.Name;
            cliente.user = user;

            db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/producto/{id}/{cant}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id, int cant)
        {
            var user = User.Identity.Name;
            Tbl_OrdenTmp orden = new Tbl_OrdenTmp();

            orden.id_Articulo = id;
            orden.cantidad = cant;
            orden.user = user;

            db.Tbl_OrdenTmp.Add(orden);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("eliminar/productoTmp/{id}")]
        [HttpGet]
        public JsonResult EliminarProducto(int id)
        {
            var articulo = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id && m.user == User.Identity.Name).FirstOrDefault();
            db.Tbl_OrdenTmp.Remove(articulo);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("eliminar/eliminarProductosTodos")]
        [HttpGet]
        public JsonResult EliminarProductosTodos()
        {
            var articulos = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name).ToList();
            db.Tbl_OrdenTmp.RemoveRange(articulos);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/productosTmp")]
        [HttpGet]
        public JsonResult MostrarProductosTmp()
        {
            var datos = (from p in db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name)
                         select new
                         {
                             ID = p.id_OrdenTmp,
                             Nombre = p.Tbl_Articulo.nombre_articulo,
                             Imagen = p.Tbl_Articulo.imagen,
                             Cantidad = p.cantidad
                         }).ToList();

            return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
        }
    }
}