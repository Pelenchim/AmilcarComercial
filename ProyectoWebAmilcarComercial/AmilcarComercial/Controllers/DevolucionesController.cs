using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    public class DevolucionesController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        #region Vistas
        // GET: Devoluciones
        public ActionResult Index()
        {
            return View();
        }

        [Route("cliente/devolucion/nueva")]
        public ActionResult NuevaCliente()
        {
            return View();
        }

        [Route("proveedor/devolucion/nueva")]
        public ActionResult NuevaProveedor()
        {
            return View();
        }

        #endregion

        #region Compras

        [Route("obtener/compras")]
        [HttpGet]
        public JsonResult ObtenerCompras()
        {
            var compras = (from c in db.Tbl_Compra.Where(m => m.usuario == User.Identity.Name).OrderByDescending(m => m.id_compra)
                               select new
                               {
                                   ID = c.id_compra,
                                   Proveedor = c.id_proveedor,
                                   Factura = c.fact_compra,
                                   Fecha = c.fecha_compra,
                                   Cantidad = c.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.cantidad)
                               }).ToList();

            return Json(new { data = compras }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/devolucion/{tipo}")]
        [HttpGet]
        public JsonResult MostrarDevolucionTmp(string tipo)
        {
            if (db.Tbl_DevolucionTmp.Where(m => m.tipo == tipo && m.user == User.Identity.Name).Count() != 0)
            {
                var data = (from d in db.Tbl_DevolucionTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo)
                            select new
                            {
                                ID = d.Tbl_Compra.id_compra,
                                Fecha = d.Tbl_Compra.fecha_compra,
                                Proveedor = d.Tbl_Compra.Tbl_Proveedor.razon_social,
                                Comprador = d.Tbl_Compra.usuario
                            }).First();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = 0;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("agregar/compra/{id}")]
        [HttpGet]
        public JsonResult AgregarCompra(int id)
        {
            var user = User.Identity.Name;
            Tbl_Compra compra = db.Tbl_Compra.Find(id);
            Tbl_DevolucionTmp devolucion = new Tbl_DevolucionTmp();
            EliminarDevolucionesTmp("Proveedor");

            devolucion.tipo = "Proveedor";
            devolucion.iva = (int)compra.iva_compra;
            devolucion.id_transaccion = id;
            devolucion.user = user;

            db.Tbl_DevolucionTmp.Add(devolucion);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("eliminar/devoluciontmp/{tipo}")]
        public JsonResult EliminarDevolucionesTmp(string tipo)
        {
            var lista = db.Tbl_DevolucionTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).ToList();

            if (lista != null)
            {
                db.Tbl_DevolucionTmp.RemoveRange(lista);
                db.SaveChanges();
            }
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Articulos

        [Route("devoluciones/obtener/productos/{id}")]
        [HttpGet]
        public JsonResult ObtenerProductos(int id)
        {
            var Articulos = (from p in db.Tbl_Detalle_Compra
                             where ( p.id_compra == id )
                             select new
                             {
                                 ID = p.id_articulo,
                                 Codigo = p.Tbl_Articulo.codigo_articulo,
                                 Nombre = p.Tbl_Articulo.nombre_articulo,
                                 Imagen = p.Tbl_Articulo.imagen,
                                 Costo = p.costo,
                                 Descuento = p.descuento,
                                 Cantidad = p.cantidad
                             }).ToList();

            return Json(new { data = Articulos }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}