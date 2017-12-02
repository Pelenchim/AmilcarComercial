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
            return RedirectToAction("ListaCliente");
        }

        public ActionResult ListaDevCliente()
        {
            return View();
        }

        public ActionResult ListaDevProveedor()
        {
            return View();
        }

        public ActionResult Consultas()
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

        #region Generales
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

        [Route("devolucion/cliente/buscar/{fact}")]
        public JsonResult buscarCliente(string fact)
        {
            if (db.Tbl_DevolucionCliente.Where(m => m.fact == fact).Count() > 0)
            {
                var id = db.Tbl_DevolucionCliente.Where(m => m.fact == fact).FirstOrDefault().id_devolucionCliente;

                var data = (from c in db.Tbl_DevolucionCliente
                            join d in db.Tbl_DetalleDevolucionCliente on c.id_devolucionCliente equals d.id_cliente
                            where c.id_devolucionCliente == id
                            select new
                            {
                                Factura = c.fact,
                                ClienteN = c.Tbl_Orden.Tbl_Clientes.nombre_cliente,
                                ClienteA = c.Tbl_Orden.Tbl_Clientes.apellidos_cliente,
                                Fecha = c.fecha.ToString(),
                                VendedorN = db.AspNetUsers.Where(m => m.UserName == c.user).FirstOrDefault().FirstName,
                                VendedorA = db.AspNetUsers.Where(m => m.UserName == c.user).FirstOrDefault().LastName,
                                CantidadTotal = db.Tbl_DetalleDevolucionCliente.Where(m => m.id_cliente == c.id_devolucionCliente).Sum(m => m.cantidad)
                            }).FirstOrDefault();

                var dato = (from c in db.Tbl_DetalleDevolucionCliente
                            where c.id_cliente == id
                            select new
                            {
                                Articulo = c.Tbl_Articulo.nombre_articulo,
                                Img = c.Tbl_Articulo.imagen,
                                Cantidad = c.cantidad,
                                Descripcion = c.descripcion,
                            }).ToList();

                var result = new { Maestro = data, Detalle = dato };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = false;

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        [Route("devolucion/proveedor/buscar/{fact}")]
        public JsonResult buscarProveedor(string fact)
        {
            if (db.Tbl_DevolucionProveedor.Where(m => m.fact == fact).Count() > 0)
            {
                var id = db.Tbl_DevolucionProveedor.Where(m => m.fact == fact).FirstOrDefault().id_DevolucionProveedor;

                var data = (from c in db.Tbl_DevolucionProveedor
                            join d in db.Tbl_DetalleDevolucionProveedor on c.id_DevolucionProveedor equals d.id_proveedor
                            where c.id_DevolucionProveedor == id
                            select new
                            {
                                Factura = c.fact,
                                Proveedor = c.Tbl_Compra.Tbl_Proveedor.razon_social,
                                Fecha = c.fecha.ToString(),
                                CompradorN = db.AspNetUsers.Where(m => m.UserName == c.user).FirstOrDefault().FirstName,
                                CompradorA = db.AspNetUsers.Where(m => m.UserName == c.user).FirstOrDefault().LastName,
                                CantidadTotal = db.Tbl_DetalleDevolucionProveedor.Where(m => m.id_proveedor == c.id_DevolucionProveedor).Sum(m => m.cantidad)
                            }).FirstOrDefault();

                var dato = (from c in db.Tbl_DetalleDevolucionProveedor
                            where c.id_proveedor == id
                            select new
                            {
                                Articulo = c.Tbl_Articulo.nombre_articulo,
                                Img = c.Tbl_Articulo.imagen,
                                Cantidad = c.cantidad,
                                Descripcion = c.descripcion,
                            }).ToList();

                var result = new { Maestro = data, Detalle = dato };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = false;

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

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
                                ID = d.Tbl_Compra.id_compra,
                                Fecha = d.Tbl_Compra.fecha_compra.ToString(),
                                Entidad = d.Tbl_Compra.Tbl_Proveedor.razon_social,
                                UserN = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Compra.usuario).FirstOrDefault().FirstName,
                                UserA = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Compra.usuario).FirstOrDefault().LastName
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
                                ID = d.Tbl_Orden.id_orden,
                                Fecha = d.Tbl_Orden.fecha_orden.ToString(),
                                EntidadN = d.Tbl_Orden.Tbl_Clientes.nombre_cliente,
                                EntidadA = d.Tbl_Orden.Tbl_Clientes.apellidos_cliente,
                                UserN = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Orden.usuario).FirstOrDefault().FirstName,
                                UserA = db.AspNetUsers.Where(m => m.UserName == d.Tbl_Orden.usuario).FirstOrDefault().LastName
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
                             where (p.id_compra == id)
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

        [Route("devoluciones/agregar/producto/{id}/{cant}/{descrip}/{tipo}/{cant2}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id, int cant, string descrip, string tipo, int cant2)
        {
            var user = User.Identity.Name;
            Tbl_DevolucionDetalleTmp devolucion = new Tbl_DevolucionDetalleTmp();

            devolucion.id_articulo = id;
            devolucion.cantidad = cant;
            devolucion.cantidadcomprada = cant2;
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
                             where (p.user == User.Identity.Name && p.tipo == tipo)
                             select new
                             {
                                 ID = p.Tbl_Articulo.id_articulo,
                                 Nombre = p.Tbl_Articulo.nombre_articulo,
                                 Imagen = p.Tbl_Articulo.imagen,
                                 Cantidad = p.cantidad,
                                 CantidadCV = p.cantidadcomprada,
                                 Descripcion = p.descripcion
                             }).ToList();

                return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var datos = 0;

                return Json(datos, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("devoluciones/cliente/cancelar")]
        [HttpPost]
        public JsonResult CancelarDevCliente()
        {
            var venta = db.Tbl_DevolucionTmp.Where(m => m.user == User.Identity.Name && m.tipo == "Cliente").ToList();
            var articulos = db.Tbl_DevolucionDetalleTmp.Where(m => m.user == User.Identity.Name && m.tipo == "Cliente").ToList();

            if (venta != null)
            {
                db.Tbl_DevolucionTmp.RemoveRange(venta);
                db.SaveChanges();
            }
            if (articulos != null)
            {
                db.Tbl_DevolucionDetalleTmp.RemoveRange(articulos);
                db.SaveChanges();
            }
            return Json(Url.Action("ListaDevCliente", "Devoluciones"));
        }

        [Route("devoluciones/proveedor/cancelar")]
        [HttpPost]
        public JsonResult CancelarDevProveedor()
        {
            var venta = db.Tbl_DevolucionTmp.Where(m => m.user == User.Identity.Name && m.tipo == "Proveedor").ToList();
            var articulos = db.Tbl_DevolucionDetalleTmp.Where(m => m.user == User.Identity.Name && m.tipo == "Proveedor").ToList();

            if (venta != null)
            {
                db.Tbl_DevolucionTmp.RemoveRange(venta);
                db.SaveChanges();
            }
            if (articulos != null)
            {
                db.Tbl_DevolucionDetalleTmp.RemoveRange(articulos);
                db.SaveChanges();
            }
            return Json(Url.Action("ListaDevProveedor", "Devoluciones"));
        }

        [Route("devoluciones/cliente/guardar")]
        [HttpGet]
        public JsonResult GuardarCli(Tbl_DevolucionCliente cliente)
        {
            bool data = false;

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;

                    Tbl_DevolucionCliente maestro = new Tbl_DevolucionCliente()
                    {
                        fecha = DateTime.Now,
                        user = User.Identity.Name,
                        fact = cliente.fact,
                        id_venta = cliente.id_venta
                    };
                    db.Tbl_DevolucionCliente.Add(maestro);
                    db.SaveChanges();

                    var detalleTmp = db.Tbl_DevolucionDetalleTmp.Where(m => m.user == User.Identity.Name && m.tipo == "Cliente").ToList();

                    foreach (var articulo in detalleTmp)
                    {
                        var stock = db.Tbl_bodega_productos.Where(m => m.id_sucursal == suc && m.id_articulo == articulo.id_articulo).FirstOrDefault();
                        stock.stock = stock.stock + articulo.cantidad;
                        stock.dañados = stock.dañados + articulo.cantidad;
                        db.SaveChanges();

                        var kardexultimo = db.Tbl_Kardex.Where(m => m.id_articulo == articulo.id_articulo).OrderByDescending(m => m.id_Kardex).FirstOrDefault();

                        Tbl_Kardex kardex = new Tbl_Kardex()
                        {
                            id_articulo = articulo.id_articulo,
                            fechaKardex = DateTime.Now,
                            num_factura = maestro.fact,
                            Entrada = articulo.cantidad,
                            salida = 0,
                            saldo = stock.stock,
                            ultimoCosto = kardexultimo.ultimoCosto,
                            costoPromedio = kardexultimo.costoPromedio,
                            usuario = User.Identity.Name,
                            id_sucursal = (int)suc,
                            tipo = "Entrada",
                            observaciones = "Devolucion Cliente"
                        };
                        db.Tbl_Kardex.Add(kardex);
                        db.SaveChanges();

                        Tbl_DetalleDevolucionCliente detalle = new Tbl_DetalleDevolucionCliente()
                        {
                            id_articulo = articulo.id_articulo,
                            descripcion = articulo.descripcion,
                            cantidad = articulo.cantidad,
                            id_kardex = kardex.id_Kardex,
                            id_cliente = maestro.id_devolucionCliente
                        };

                        db.Tbl_DetalleDevolucionCliente.Add(detalle);
                        db.SaveChanges();
                    }
                    db.Tbl_DevolucionDetalleTmp.RemoveRange(detalleTmp);
                    db.SaveChanges();

                    tran.Commit();
                    data = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Route("devoluciones/proveedor/guardar")]
        [HttpGet]
        public JsonResult GuardarPro(Tbl_DevolucionProveedor proveedor)
        {
            bool data = false;

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;

                    Tbl_DevolucionProveedor maestro = new Tbl_DevolucionProveedor()
                    {
                        fecha = DateTime.Now,
                        user = User.Identity.Name,
                        fact = proveedor.fact,
                        id_compra = proveedor.id_compra
                    };
                    db.Tbl_DevolucionProveedor.Add(maestro);
                    db.SaveChanges();

                    var detalleTmp = db.Tbl_DevolucionDetalleTmp.Where(m => m.user == User.Identity.Name && m.tipo == "Proveedor").ToList();

                    foreach (var articulo in detalleTmp)
                    {
                        var stock = db.Tbl_bodega_productos.Where(m => m.id_sucursal == suc && m.id_articulo == articulo.id_articulo).FirstOrDefault();
                        stock.dañados = stock.dañados - articulo.cantidad;
                        stock.stock = stock.stock - articulo.cantidad;
                        db.SaveChanges();

                        Tbl_Kardex kardex = new Tbl_Kardex()
                        {
                            id_articulo = articulo.id_articulo,
                            fechaKardex = DateTime.Now,
                            num_factura = maestro.fact,
                            Entrada = 0,
                            salida = articulo.cantidad,
                            saldo = stock.stock,
                            ultimoCosto = 0,
                            costoPromedio = 0,
                            usuario = User.Identity.Name,
                            id_sucursal = (int)suc,
                            tipo = "Salida",
                            observaciones = "Devolucion Proveedor"
                        };
                        db.Tbl_Kardex.Add(kardex);
                        db.SaveChanges();

                        Tbl_DetalleDevolucionProveedor detalle = new Tbl_DetalleDevolucionProveedor()
                        {
                            id_articulo = articulo.id_articulo,
                            descripcion = articulo.descripcion,
                            cantidad = articulo.cantidad,
                            id_kardex = kardex.id_Kardex,
                            id_proveedor = maestro.id_DevolucionProveedor
                        };

                        db.Tbl_DetalleDevolucionProveedor.Add(detalle);
                        db.SaveChanges();
                    }
                    db.Tbl_DevolucionDetalleTmp.RemoveRange(detalleTmp);
                    db.SaveChanges();

                    tran.Commit();
                    data = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ListaDevoluciones

        [Route("devolucion/cliente/lista")]
        [HttpGet]
        public JsonResult ListaCliente()
        {
            var lista = (from d in db.Tbl_DevolucionCliente
                         select new
                         {
                             ID = d.id_devolucionCliente,
                             Fecha = d.fecha.ToString(),
                             Factura = d.fact,
                             ClienteN = d.Tbl_Orden.Tbl_Clientes.nombre_cliente,
                             ClienteA = d.Tbl_Orden.Tbl_Clientes.apellidos_cliente,
                             Articulos = d.Tbl_DetalleDevolucionCliente.Where(m => m.id_cliente == d.id_devolucionCliente).Count(),
                             Cantidad = d.Tbl_DetalleDevolucionCliente.Where(m => m.id_detalleDevolucionCli == d.id_devolucionCliente).Sum(m => m.cantidad)
                         }).OrderByDescending(m => m.ID).Take(10).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [Route("devolucion/proveedor/lista")]
        [HttpGet]
        public JsonResult ListaProveedor()
        {
            var lista = (from d in db.Tbl_DevolucionProveedor
                         select new
                         {
                             ID = d.id_DevolucionProveedor,
                             Fecha = d.fecha.ToString(),
                             Factura = d.fact,
                             Proveedor = d.Tbl_Compra.Tbl_Proveedor.razon_social,
                             Articulos = d.Tbl_DetalleDevolucionProveedor.Where(m => m.id_proveedor == d.id_DevolucionProveedor).Count(),
                             Cantidad = d.Tbl_DetalleDevolucionProveedor.Where(m => m.id_proveedor == d.id_DevolucionProveedor).Sum(m => m.cantidad)
                         }).OrderByDescending(m => m.ID).Take(10).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [Route("devolucion/cliente/detalle/general/{id}")]
        [HttpGet]
        public JsonResult DetalleClienteGeneral(int id)
        {
            var detalle = from c in db.Tbl_DevolucionCliente
                          where c.id_devolucionCliente == id
                          select new
                          {
                              Usuario = c.user,
                              Devolucion = c.id_devolucionCliente,
                              Factura = c.fact,
                              Fecha = c.fecha.ToString(),
                              ClienteN = c.Tbl_Orden.Tbl_Clientes.nombre_cliente,
                              ClienteA = c.Tbl_Orden.Tbl_Clientes.apellidos_cliente,
                              Articulos = db.Tbl_DetalleDevolucionCliente.Where(m => m.id_cliente == c.id_devolucionCliente).Count(),
                              CantidadTotal = db.Tbl_DetalleDevolucionCliente.Where(m => m.id_cliente == c.id_devolucionCliente).Sum(m => m.cantidad)
                          };

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        }

        [Route("devolucion/cliente/detalle/especifico/{id}")]
        [HttpGet]
        public JsonResult DetalleClienteEspecifico(int id)
        {
            var detalle = (from c in db.Tbl_DetalleDevolucionCliente
                           where c.id_cliente == id
                           select new
                           {
                               Articulo = c.Tbl_Articulo.nombre_articulo,
                               Img = c.Tbl_Articulo.imagen,
                               Categoria = c.Tbl_Articulo.Tbl_Categorias.Nombre,
                               Cantidad = c.cantidad,
                               Descripcion = c.descripcion
                           }).ToList();

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        }

        [Route("devolucion/proveedor/detalle/general/{id}")]
        [HttpGet]
        public JsonResult DetalleProveedorGeneral(int id)
        {
            var detalle = from c in db.Tbl_DevolucionProveedor
                          where c.id_DevolucionProveedor == id
                          select new
                          {
                              Usuario = c.user,
                              Devolucion = c.id_DevolucionProveedor,
                              Factura = c.fact,
                              Fecha = c.fecha.ToString(),
                              Proveedor = c.Tbl_Compra.Tbl_Proveedor.razon_social,
                              Articulos = db.Tbl_DetalleDevolucionProveedor.Where(m => m.id_proveedor == c.id_DevolucionProveedor).Count(),
                              CantidadTotal = db.Tbl_DetalleDevolucionProveedor.Where(m => m.id_proveedor == c.id_DevolucionProveedor).Sum(m => m.cantidad)
                          };

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        }

        [Route("devolucion/proveedor/detalle/especifico/{id}")]
        [HttpGet]
        public JsonResult DetalleProveedorEspecifico(int id)
        {
            var detalle = (from c in db.Tbl_DetalleDevolucionProveedor
                           where c.id_proveedor == id
                           select new
                           {
                               Articulo = c.Tbl_Articulo.nombre_articulo,
                               Img = c.Tbl_Articulo.imagen,
                               Categoria = c.Tbl_Articulo.Tbl_Categorias.Nombre,
                               Cantidad = c.cantidad,
                               Descripcion = c.descripcion
                           }).ToList();

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}