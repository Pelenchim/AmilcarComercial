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
            var ventas = (from c in db.Tbl_Orden.OrderByDescending(m => m.id_orden)
                           select new
                           {
                               ID = c.id_orden,
                               ClienteApellido = c.Tbl_Clientes.apellidos_cliente,
                               ClienteNombre = c.Tbl_Clientes.nombre_cliente,
                               Factura = c.fact_Orden,
                               Fecha = c.fecha_orden,
                               Cantidad = c.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Sum(m => m.cantidad)
                           }).ToList();

            return Json(new { data = ventas }, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/devoluciontmpProveedor/{id}")]
        [HttpGet]
        public JsonResult AgregarDevolucionTmpProveedor(int id)
        {
            var user = User.Identity.Name;
            Tbl_DevolucionTmp devolucion = new Tbl_DevolucionTmp();
            EliminarDevolucionesTmp("Proveedor");

            devolucion.tipo = "Proveedor";
            devolucion.id_compra = id;
            devolucion.user = user;

            db.Tbl_DevolucionTmp.Add(devolucion);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/devoluciontmpCliente/{id}")]
        [HttpGet]
        public JsonResult AgregarDevolucionTmpCliente(int id)
        {
            var user = User.Identity.Name;
            Tbl_DevolucionTmp devolucion = new Tbl_DevolucionTmp();
            EliminarDevolucionesTmp("Cliente");

            devolucion.tipo = "Cliente";
            devolucion.id_venta = id;
            devolucion.user = user;

            db.Tbl_DevolucionTmp.Add(devolucion);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/devoluciontmpProveedor/{tipo}")]
        [HttpGet]
        public JsonResult MostrarDevolucionTmpProveedor(string tipo)
        {
            if (db.Tbl_DevolucionTmp.Where(m => m.tipo == tipo && m.user == User.Identity.Name).Count() != 0)
            {
                var data = (from d in db.Tbl_DevolucionTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo)
                            select new
                            {
                                ID = d.Tbl_Compra1.id_compra,
                                Fecha = d.Tbl_Compra1.fecha_compra.ToString(),
                                Entidad = d.Tbl_Compra1.Tbl_Proveedor.razon_social,
                                UserN = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Compra1.usuario).FirstOrDefault().FirstName,
                                UserA = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Compra1.usuario).FirstOrDefault().LastName
                            }).First();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = 0;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("obtener/devoluciontmpCliente/{tipo}")]
        [HttpGet]
        public JsonResult MostrarDevolucionTmpCliente(string tipo)
        {
            if (db.Tbl_DevolucionTmp.Where(m => m.tipo == tipo && m.user == User.Identity.Name).Count() != 0)
            {
                var data = (from d in db.Tbl_DevolucionTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo)
                            select new
                            {
                                ID = d.Tbl_Orden1.id_orden,
                                Fecha = d.Tbl_Orden1.fecha_orden.ToString(),
                                EntidadN = d.Tbl_Orden1.Tbl_Clientes.nombre_cliente,
                                EntidadA = d.Tbl_Orden1.Tbl_Clientes.apellidos_cliente,
                                UserN = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Orden1.usuario).FirstOrDefault().FirstName,
                                UserA = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Orden1.usuario).FirstOrDefault().LastName
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

        [Route("devoluciones/obtener/productosCompra/{id}")]
        [HttpGet]
        public JsonResult ObtenerProductosCompra(int id)
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

        [Route("devoluciones/obtener/productosVenta/{id}")]
        [HttpGet]
        public JsonResult ObtenerProductosVenta(int id)
        {
            var Articulos = (from p in db.Tbl_Detalle_Orden
                             where (p.id_orden == id)
                             select new
                             {
                                 ID = p.id_articulo,
                                 Codigo = p.Tbl_Articulo.codigo_articulo,
                                 Nombre = p.Tbl_Articulo.nombre_articulo,
                                 Imagen = p.Tbl_Articulo.imagen,
                                 Precio = p.precio_venta,
                                 Descuento = p.descuento,
                                 Cantidad = p.cantidad
                             }).ToList();

            return Json(new { data = Articulos }, JsonRequestBehavior.AllowGet);
        }

        [Route("devoluciones/agregar/producto/{id}/{cant}/{descrip}/{tipo}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id, int cant, string descrip, string tipo)
        {
            var user = User.Identity.Name;
            Tbl_DevolucionDetalleTmp devolucion = new Tbl_DevolucionDetalleTmp();

            devolucion.id_articulo = id;
            devolucion.cantidad = cant;
            devolucion.descripcion = descrip;
            devolucion.user = user;
            devolucion.tipo = tipo;

            db.Tbl_DevolucionDetalleTmp.Add(devolucion);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("devoluciones/eliminar/productoTmp/{id}/{tipo}")]
        [HttpGet]
        public JsonResult EliminarProducto(int id, string tipo)
        {
            var articulo = db.Tbl_DevolucionDetalleTmp.Where(m => m.id_articulo == id && m.user == User.Identity.Name && m.tipo == tipo).FirstOrDefault();
            db.Tbl_DevolucionDetalleTmp.Remove(articulo);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("devoluciones/eliminar/eliminarProductosTodos/{tipo}")]
        [HttpGet]
        public JsonResult EliminarProductosTodos(string tipo)
        {
            var articulos = db.Tbl_DevolucionDetalleTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).ToList();
            db.Tbl_DevolucionDetalleTmp.RemoveRange(articulos);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("devoluciones/obtener/productosTmp/{tipo}")]
        [HttpGet]
        public JsonResult MostrarProductosTmp(string tipo)
        {
            if (db.Tbl_DevolucionDetalleTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).Count() != 0)
            {
                var datos = (from p in db.Tbl_DevolucionDetalleTmp
                             join b in db.Tbl_bodega_productos on p.id_articulo equals b.id_articulo
                             where (p.user == User.Identity.Name && p.tipo == tipo)
                             select new
                             {
                                 ID = p.Tbl_Articulo.id_articulo,
                                 Nombre = p.Tbl_Articulo.nombre_articulo,
                                 Imagen = p.Tbl_Articulo.imagen,
                                 Cantidad = p.cantidad,
                                 Existecia = b.stock,
                                 Precio = b.precio
                             }).ToList();

                return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var datos = 0;

                return Json(datos, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}