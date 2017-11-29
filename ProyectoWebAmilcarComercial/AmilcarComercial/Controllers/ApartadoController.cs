using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
    public class ApartadoController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        GeneralesController generales = new GeneralesController();

        #region vistas
        // GET: Apartado
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Nueva()
        {
            return View();
        }
        [Route("apartado/facturado")]
        public ActionResult Facturado()
        {
            return View();
        }

        public ActionResult Abono()
        {
            return View();
        }

        public ActionResult Consulta()
        {
            return View();
        }

        #endregion

        [Route("apartado/obtener/generales")]
        [HttpGet]
        public JsonResult Generales()
        {
            var ID_sucursal = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
            var sucursal = db.Tbl_Sucursal.FirstOrDefault(m => m.id_sucursal == ID_sucursal).Nombre;
            var venta = (db.Tbl_Orden.OrderByDescending(m => m.id_orden).First().id_orden + 1).ToString();
            var UserLastName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().LastName;
            var UserFirtsName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().FirstName;
            var fact = String.Concat("Apart" + venta);

            List<string> lista = new List<string>();
            lista.Add(DateTime.Now.ToString());
            lista.Add((UserFirtsName + ' ' + UserLastName).ToString());
            lista.Add(sucursal);
            lista.Add(venta);
            lista.Add(fact);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/anular/{id}")]
        [HttpGet]
        public JsonResult AnularVenta(int id)
        {
            var dato = false;

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;

                    var orden = db.Tbl_Orden.Where(m => m.id_orden == id).FirstOrDefault();
                    orden.estado = false;
                    db.SaveChanges();

                    var detalleOrden = db.Tbl_Detalle_Orden.Where(m => m.id_orden == orden.id_orden).ToList();

                    foreach (var articulo in detalleOrden)
                    {
                        var stock = db.Tbl_bodega_productos.Where(m => m.id_sucursal == suc && m.id_articulo == articulo.id_articulo).FirstOrDefault();
                        stock.stock = (int)stock.stock + (int)articulo.cantidad;
                        db.SaveChanges();

                        Tbl_Kardex kardex = new Tbl_Kardex()
                        {
                            id_articulo = (int)articulo.id_articulo,
                            fechaKardex = DateTime.Today,
                            num_factura = orden.fact_Orden,
                            salida = 0,
                            Entrada = (int)articulo.cantidad,
                            saldo = (int)stock.stock,
                            ultimoCosto = 0,
                            costoPromedio = 0,
                            usuario = User.Identity.Name,
                            id_sucursal = (int)suc,
                            tipo = "Entrada",
                            observaciones = "Venta-Apartado-Anulada"
                        };
                        db.Tbl_Kardex.Add(kardex);
                        db.SaveChanges();
                    }
                    db.SaveChanges();

                    tran.Commit();
                    dato = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            return Json(dato, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/detallebusqueda/{fact}")]
        public JsonResult detalleBusqueda(string fact)
        {
            var id = db.Tbl_Compra.Where(m => m.fact_compra == fact).FirstOrDefault().id_compra;
            var data = (from c in db.Tbl_Detalle_Compra
                        where c.id_compra == id
                        select new
                        {
                            Articulo = c.Tbl_Articulo.nombre_articulo,
                            Img = c.Tbl_Articulo.imagen,
                            Cantidad = c.cantidad,
                            Descuento = c.descuento,
                            Costo = c.costo,
                            Subtotal = c.costo * c.cantidad - c.descuento
                        }).ToList();

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        #region Articulos

        [Route("apartado/obtener/articulos")]
        [HttpGet]
        public JsonResult ObtenerArticulos()
        {
            var articulosOrden = (from p in db.Tbl_OrdenTmp
                                  where (p.user == User.Identity.Name && p.tipoventa == "Apartado")
                                  select p.id_Articulo).ToArray();

            var sucursal = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Sucursal;

            var Articulos = (from p in db.Tbl_Articulo join b in db.Tbl_bodega_productos on p.id_articulo equals b.id_articulo
                             where (b.apartado == true && b.id_sucursal == sucursal && p.estado == true &&
                                    !(articulosOrden.Contains((int)p.id_articulo)))
                             select new
                             {
                                 ID = p.id_articulo,
                                 Codigo = p.codigo_articulo,
                                 Nombre = p.nombre_articulo,
                                 Imagen = p.imagen,
                                 Stock = b.stock,
                                 Precio = b.precio,
                                 Prima = b.precio * 0.20
                             }).ToList();

            return Json(new { data = Articulos }, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/agregar/producto/{id}/{cant}/{prima}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id, int cant, float prima)
        {
            var user = User.Identity.Name;
            Tbl_OrdenTmp orden = new Tbl_OrdenTmp();

            orden.id_Articulo = id;
            orden.cantidad = cant;
            orden.fecha = DateTime.Now;
            orden.prima = prima * cant;
            orden.user = user;
            orden.tipoventa = "Apartado";

            db.Tbl_OrdenTmp.Add(orden);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/eliminar/productoTmp/{id}")]
        [HttpGet]
        public JsonResult EliminarProducto(int id)
        {
            var articulo = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id && m.user == User.Identity.Name && m.tipoventa == "Apartado").FirstOrDefault();
            db.Tbl_OrdenTmp.Remove(articulo);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/eliminar/eliminarProductosTodos")]
        [HttpGet]
        public JsonResult EliminarProductosTodos()
        {
            var articulos = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Apartado").ToList();
            db.Tbl_OrdenTmp.RemoveRange(articulos);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/obtener/productosTmp")]
        [HttpGet]
        public JsonResult MostrarProductosTmp()
        {
            if (db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Apartado").Count() != 0)
            {
                var fechafin = DateTime.Today.ToShortDateString();

                var datos = (from p in db.Tbl_OrdenTmp join b in db.Tbl_bodega_productos on p.id_Articulo equals b.id_articulo
                             where (p.user == User.Identity.Name && p.tipoventa == "Apartado")
                             select new
                             {
                                 ID = p.id_OrdenTmp,
                                 Nombre = p.Tbl_Articulo.nombre_articulo,
                                 Imagen = p.Tbl_Articulo.imagen,
                                 Cantidad = p.cantidad,
                                 Existecia = b.stock,
                                 Precio = b.precio,
                                 PrimaMinima = b.precio * 0.20,
                                 Prima = p.prima,
                                 FechaLimite = fechafin
                             }).ToList();

                return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var datos = 0;

                return Json(datos, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("apartado/actualizar/cantidad/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarCantidad(int id, int nuevoValor)
        {
            var dato = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id).FirstOrDefault();
            dato.cantidad = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Facturacion

        [Route("apartado/facturar")]
        [HttpGet]
        public JsonResult Facturar(Tbl_Orden venta)
        {
            var dato = false;

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
                    var fecha = DateTime.Now;
                    var cliente = guardarCliente("Apartado");

                    Tbl_Orden maestro = new Tbl_Orden()
                    {
                        id_sucursal = (int)suc,
                        usuario = User.Identity.Name,
                        fecha_orden = fecha,
                        iva_orden = venta.iva_orden,
                        estado = true,
                        tipo_orden = "Apartado",
                        fact_Orden = venta.fact_Orden,
                        id_cliente = cliente,
                        tipo_pago = venta.tipo_pago
                    };
                    db.Tbl_Orden.Add(maestro);
                    db.SaveChanges();

                    var detalleTmp = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Apartado").ToList();

                    foreach (var articulo in detalleTmp)
                    {
                        var stock = db.Tbl_bodega_productos.Where(m => m.id_sucursal == suc && m.id_articulo == articulo.id_Articulo).FirstOrDefault();
                        stock.stock = (int)stock.stock - (int)articulo.cantidad;
                        db.SaveChanges();

                        Tbl_Kardex kardex = new Tbl_Kardex()
                        {
                            id_articulo = (int)articulo.id_Articulo,
                            fechaKardex = (DateTime)articulo.fecha,
                            num_factura = maestro.fact_Orden,
                            Entrada = 0,
                            salida = (int)articulo.cantidad,
                            saldo = (int)stock.stock,
                            ultimoCosto = 0,
                            costoPromedio = 0,
                            usuario = User.Identity.Name,
                            id_sucursal = (int)suc,
                            tipo = "Salida",
                            observaciones = "Venta-Apartado-Aprovada"
                        };
                        db.Tbl_Kardex.Add(kardex);
                        db.SaveChanges();

                        Tbl_Detalle_Orden detalle = new Tbl_Detalle_Orden()
                        {
                            id_orden = maestro.id_orden,
                            id_articulo = (int)articulo.id_Articulo,
                            id_kardex = kardex.id_Kardex,
                            cantidad = (int)articulo.cantidad,
                            precio_venta = 0,
                            descuento = 0
                        };
                        db.Tbl_Detalle_Orden.Add(detalle);
                        db.SaveChanges();
                    }
                    db.Tbl_OrdenTmp.RemoveRange(detalleTmp);
                    db.SaveChanges();

                    tran.Commit();
                    dato = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            return Json(dato, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/cancelar")]
        [HttpPost]
        public JsonResult CancelarCompra()
        {
            var cliente = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipo == "Apartado").ToList();
            var articulos = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Apartado").ToList();

            if (cliente != null)
            {
                db.Tbl_ClienteTmp.RemoveRange(cliente);
                db.SaveChanges();
            }
            if (articulos != null)
            {
                db.Tbl_OrdenTmp.RemoveRange(articulos);
                db.SaveChanges();
            }

            return Json(Url.Action("Index", "Apartado"));
        }

        [Route("apartado/detallefactura")]
        public ActionResult Detalle()
        {
            var ultimo = db.Tbl_Orden.Where(m => m.usuario == User.Identity.Name && m.tipo_orden == "Apartado")
                       .OrderByDescending(m => m.id_orden).First();

            var cantidad = db.Tbl_Detalle_Orden.Where(m => m.id_orden == ultimo.id_orden).Sum(m => m.cantidad);
            var subtotal = (from v in db.Tbl_Detalle_Orden
                            where v.id_orden == ultimo.id_orden
                            select new
                            {
                                sub = (v.precio_venta - v.descuento) * v.cantidad
                            }).Sum(m => m.sub);
            var descuento = db.Tbl_Detalle_Orden.Where(m => m.id_orden == ultimo.id_orden).Sum(m => m.descuento);
            var ivatotal = subtotal * (ultimo.iva_orden / 100);

            var total = subtotal + ivatotal - descuento;
            var id = ultimo.id_orden;

            List<double> datos = new List<double>();
            datos.Add(cantidad);
            datos.Add((float)subtotal);
            datos.Add((float)ivatotal);
            datos.Add((float)descuento);
            datos.Add((float)total);
            datos.Add(id);

            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        public int guardarCliente(string tipo)
        {
            var nuevo = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).FirstOrDefault().nuevo;

            if (nuevo == true)
            {
                var clienteTmp = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).FirstOrDefault();

                Tbl_Clientes cliente = new Tbl_Clientes()
                {
                    nombre_cliente = clienteTmp.nombre_cliente,
                    apellidos_cliente = clienteTmp.apellidos_cliente,
                    direccion = clienteTmp.direccion,
                    departamento = clienteTmp.departamento,
                    telefono = (int)clienteTmp.telefono,
                    cedula = clienteTmp.cedula,
                    estado = true
                };
                db.Tbl_Clientes.Add(cliente);
                db.Tbl_ClienteTmp.Remove(clienteTmp);
                db.SaveChanges();

                var ultimo = db.Tbl_Clientes.OrderByDescending(m => m.id_cliente).FirstOrDefault().id_cliente;
                return ultimo;
            }
            else
            {
                var clienteTmp = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).FirstOrDefault();
                var id = clienteTmp.id_cliente;

                var cliente = db.Tbl_Clientes.Find(id).id_cliente;
                db.Tbl_ClienteTmp.Remove(clienteTmp);
                db.SaveChanges();

                return cliente;
            }
        }

        #endregion

        #region ListaVentas
        [Route("apartado/listaventas")]
        [HttpGet]
        public JsonResult ListaVentas()
        {
            var ventas = (from v in db.Tbl_Orden
                          where v.usuario == User.Identity.Name && v.tipo_orden == "Apartado"
                          select new
                          {
                              ID = v.id_orden,
                              Venta = v.id_orden,
                              Fecha = v.fecha_orden.ToString(),
                              Factura = v.fact_Orden,
                              Tipo = v.tipo_orden,
                              ClienteNom = v.Tbl_Clientes.nombre_cliente,
                              ClienteApell = v.Tbl_Clientes.apellidos_cliente,
                              Articulos = db.Tbl_Detalle_Orden.Where(m => m.id_orden == v.id_orden).Count(),
                              PagoTotal = db.Tbl_Detalle_Orden.Where(m => m.id_orden == v.id_orden).Sum(m => m.precio_venta),
                              Estado = v.estado
                          }).OrderByDescending(m => m.Fecha).ToList();

            return Json(new { data = ventas }, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/detalleventa/general/{id}")]
        [HttpGet]
        public JsonResult DetalleVentaGeneral(int id)
        {
            var detalle = from c in db.Tbl_Orden
                          where c.usuario == User.Identity.Name && c.id_orden == id
                          select new
                          {
                              Usuario = c.usuario,
                              Venta = c.id_orden,
                              Factura = c.fact_Orden,
                              Fecha = c.fecha_orden.ToString(),
                              ClienteN = c.Tbl_Clientes.nombre_cliente,
                              ClienteA = c.Tbl_Clientes.apellidos_cliente,
                              Articulos = db.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Count(),
                              CantidadTotal = db.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Sum(m => m.cantidad),
                              Iva = c.iva_orden,
                              DescuentoTotal = db.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Sum(m => m.descuento),
                              SubTotal = db.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Sum(m => m.precio_venta),
                              PagoTotal = db.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Sum(m => m.precio_venta) * db.Tbl_Detalle_Orden.Where(m => m.id_orden == c.id_orden).Sum(m => m.cantidad),
                              Sucursal = c.Tbl_Sucursal.Nombre,
                              Estado = c.estado
                          };

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        }

        [Route("apartado/detalleventa/especifico/{id}")]
        [HttpGet]
        public JsonResult DetalleVentaEspecifico(int id)
        {
            var detalle = (from c in db.Tbl_Detalle_Orden
                           where c.id_orden == id
                           select new
                           {
                               Articulo = c.Tbl_Articulo.nombre_articulo,
                               Img = c.Tbl_Articulo.imagen,
                               Categoria = c.Tbl_Articulo.Tbl_Categorias.Nombre,
                               Cantidad = c.cantidad,
                               Descuento = c.descuento,
                               Precio = c.precio_venta,
                               SubTotal = c.cantidad * c.precio_venta,
                               Total = ((c.cantidad * c.precio_venta) - c.descuento) * c.Tbl_Orden.iva_orden
                           }).ToList();

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        } 
        #endregion
    }
}