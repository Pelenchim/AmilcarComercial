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

        [Route("devolucion/proveedor/generales")]
        [HttpGet]
        public JsonResult GeneralesPro()
        {
            var ID_sucursal = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
            var sucursal = db.Tbl_Sucursal.FirstOrDefault(m => m.id_sucursal == ID_sucursal).Nombre;
            var devolucion = (db.Tbl_DevolucionProveedor.OrderByDescending(m => m.id_DevolucionProveedor).First().id_DevolucionProveedor + 1).ToString();
            var UserLastName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().LastName;
            var UserFirtsName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().FirstName;
            var fact = String.Concat("Dev-Proveedor" + devolucion);

            List<string> lista = new List<string>();
            lista.Add(DateTime.Now.ToString());
            lista.Add((UserFirtsName + ' ' + UserLastName).ToString());
            lista.Add(sucursal);
            lista.Add(devolucion);
            lista.Add(fact);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [Route("devolucion/cliente/generales")]
        [HttpGet]
        public JsonResult GeneralesCli()
        {
            var ID_sucursal = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
            var sucursal = db.Tbl_Sucursal.FirstOrDefault(m => m.id_sucursal == ID_sucursal).Nombre;
            var devolucion = (db.Tbl_DevolucionCliente.OrderByDescending(m => m.id_devolucionCliente).First().id_devolucionCliente + 1).ToString();
            var UserLastName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().LastName;
            var UserFirtsName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().FirstName;
            var fact = String.Concat("Dev-cliente" + devolucion);

            List<string> lista = new List<string>();
            lista.Add(DateTime.Now.ToString());
            lista.Add((UserFirtsName + ' ' + UserLastName).ToString());
            lista.Add(sucursal);
            lista.Add(devolucion);
            lista.Add(fact);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        #region Temporales

        [Route("obtener/compras")]
        [HttpGet]
        public JsonResult ObtenerCompras()
        {
            var compras = (from c in db.Tbl_Compra.Where(m => m.usuario == User.Identity.Name).OrderByDescending(m => m.id_compra)
                               select new
                               {
                                   ID = c.id_compra,
                                   Proveedor = c.Tbl_Proveedor.razon_social,
                                   Factura = c.fact_compra,
                                   Fecha = c.fecha_compra,
                                   Cantidad = c.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.cantidad)
                               }).ToList();

            return Json(new { data = compras }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/ventas")]
        [HttpGet]
        public JsonResult ObtenerVentas()
        {
            var compras = (from c in db.Tbl_Orden.OrderByDescending(m => m.id_orden)
                           select new
                           {
                               ID = c.id_orden,
                               ClienteApellido = c.Tbl_Clientes.apellidos_cliente,
                               ClienteNombre = c.Tbl_Clientes.nombre_cliente,
                               Factura = c.fact_Orden,
                               Fecha = c.fecha_orden,
                               Cantidad = c.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Sum(m => m.cantidad)
                           }).ToList();

            return Json(new { data = compras }, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/devoluciontmp/{id}/{tipo}")]
        [HttpGet]
        public JsonResult AgregarDevolucionTmp(int id, string tipo)
        {
            var user = User.Identity.Name;
            Tbl_Compra compra = db.Tbl_Compra.Find(id);
            Tbl_DevolucionTmp devolucion = new Tbl_DevolucionTmp();
            EliminarDevolucionesTmp(tipo);

            devolucion.tipo = tipo;
            devolucion.iva = 10;
            devolucion.id_transaccion = id;
            devolucion.user = user;

            db.Tbl_DevolucionTmp.Add(devolucion);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/devoluciontmp/{tipo}")]
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