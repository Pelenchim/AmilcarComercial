using AmilcarComercial.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        #region Vistas
        // GET: Compras
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nueva()
        {
            return View();
        }

        public ActionResult Facturado()
        {
            return View();
        }

        public ActionResult Consulta()
        {
            return View();
        }

        #endregion

        #region Generales

        [Route("compra/obtener/generales")]
        [HttpGet]
        public JsonResult Generales()
        {
            var ID_sucursal = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
            var sucursal = db.Tbl_Sucursal.FirstOrDefault(m => m.id_sucursal == ID_sucursal).Nombre;
            var compra = (db.Tbl_Compra.OrderByDescending(m => m.id_compra).First().id_compra + 1).ToString();
            var UserLastName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().LastName;
            var UserFirtsName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().FirstName;
            var fact = String.Concat("Comp" + compra);

            List<string> lista = new List<string>();
            lista.Add(DateTime.Now.ToString());
            lista.Add((UserFirtsName + ' ' + UserLastName).ToString());
            lista.Add(sucursal);
            lista.Add(compra);
            lista.Add(fact);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [Route("compra/anular/{id}")]
        [HttpGet]
        public JsonResult AnularCompra(int id)
        {
            var dato = false;

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;

                    var compra = db.Tbl_Compra.Where(m => m.id_compra == id).FirstOrDefault();
                    compra.estado_compra = false;
                    db.SaveChanges();

                    var detalleCompra = db.Tbl_Detalle_Compra.Where(m => m.id_compra == compra.id_compra).ToList();

                    foreach (var articulo in detalleCompra)
                    {
                        var stock = db.Tbl_bodega_productos.Where(m => m.id_sucursal == suc && m.id_articulo == articulo.id_articulo).FirstOrDefault();
                        stock.stock = (int)stock.stock - (int)articulo.cantidad;
                        db.SaveChanges();

                        Tbl_Kardex kardex = new Tbl_Kardex()
                        {
                            id_articulo = (int)articulo.id_articulo,
                            fechaKardex = DateTime.Today,
                            num_factura = compra.fact_compra,
                            Entrada = 0,
                            salida = (int)articulo.cantidad,
                            saldo = (int)stock.stock,
                            ultimoCosto = 0,
                            costoPromedio = 0,
                            usuario = User.Identity.Name,
                            id_sucursal = (int)suc,
                            tipo = "Salida",
                            observaciones = "Compra Anulada"
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

        [Route("compra/buscar/{fact}")]
        public JsonResult buscar(string fact)
        {
            if (db.Tbl_Compra.Where(m => m.fact_compra == fact).Count() > 0)
            {
                var id = db.Tbl_Compra.Where(m => m.fact_compra == fact).FirstOrDefault().id_compra;

                var data = (from c in db.Tbl_Compra
                            join d in db.Tbl_Detalle_Compra on c.id_compra equals d.id_compra
                            where c.id_compra == id
                            select new
                            {
                                Factura = c.fact_compra,
                                Proveedor = c.Tbl_Proveedor.razon_social,
                                Fecha = c.fecha_compra.ToString(),
                                Iva = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.costo) * (c.iva_compra / 100),
                                CompradorN = db.AspNetUsers.Where(m => m.UserName == c.usuario).FirstOrDefault().FirstName,
                                CompradorA = db.AspNetUsers.Where(m => m.UserName == c.usuario).FirstOrDefault().LastName,
                                Estado = c.estado_compra,
                                Subtotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.costo),
                                Total = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.costo) + (db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.costo) * (c.iva_compra / 100)),
                                CantidadTotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.cantidad)
                            }).FirstOrDefault();

                var dato = (from c in db.Tbl_Detalle_Compra
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

        #region Articulos         

        [Route("compras/obtener/productos/")]
        [HttpGet]
        public JsonResult ObtenerProductos()
        {
            var articulosOrden = (from p in db.Tbl_CompraTmp where (p.user == User.Identity.Name) select p.id_articulo).ToArray();

            var Articulos = (from p in db.Tbl_Articulo
                             where (!(articulosOrden.Contains((int)p.id_articulo)) && p.estado == true)
                             select new
                             {
                                 ID = p.id_articulo,
                                 Codigo = p.codigo_articulo,
                                 Nombre = p.nombre_articulo,
                                 Imagen = p.imagen
                             }).ToList();

            return Json(new { data = Articulos }, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/agregar/producto/{id}/{cant}/{precio}/{desc}/{precioventa}/{motivo}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id, int cant, float precio, float desc, float precioventa, int motivo)
        {
            var user = User.Identity.Name;

            Tbl_CompraTmp compra = new Tbl_CompraTmp();
            compra.id_articulo = id;
            compra.cantidad = cant;
            compra.costo = precio;
            compra.descuento = desc;
            compra.precioventa = precioventa;
            compra.user = user;
            compra.motivo = motivo;
            compra.fecha = System.DateTime.Now;

            db.Tbl_CompraTmp.Add(compra);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/eliminar/productoTmp/{id}")]
        [HttpGet]
        public JsonResult EliminarProducto(int id)
        {
            var articulo = db.Tbl_CompraTmp.Where(m => m.id_compraTmp == id && m.user == User.Identity.Name).FirstOrDefault();
            db.Tbl_CompraTmp.Remove(articulo);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/eliminar/eliminarProductosTodos")]
        [HttpGet]
        public JsonResult EliminarProductosTodos()
        {
            var articulos = db.Tbl_CompraTmp.Where(m => m.user == User.Identity.Name).ToList();
            db.Tbl_CompraTmp.RemoveRange(articulos);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/obtener/productosTmp")]
        [HttpGet]
        public JsonResult MostrarProductosTmp()
        {
            if (db.Tbl_CompraTmp.Where(m => m.user == User.Identity.Name).Count() != 0)
            {
                var datos = (from p in db.Tbl_CompraTmp.Where(m => m.user == User.Identity.Name)
                             select new
                             {
                                 ID = p.id_compraTmp,
                                 Nombre = p.Tbl_Articulo.nombre_articulo,
                                 Imagen = p.Tbl_Articulo.imagen,
                                 Cantidad = p.cantidad,
                                 Precio = p.costo,
                                 Descuento = p.descuento,
                                 PrecioVenta = p.precioventa,
                                 Motivo = p.motivo
                             }).ToList();
                
                return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var datos = 0;

                return Json(datos, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("compras/actualizar/costo/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarCosto(int id, int nuevoValor)
        {
            var dato = db.Tbl_CompraTmp.Where(m => m.id_compraTmp == id).FirstOrDefault();
            dato.costo = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/actualizar/cantidad/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarCantidad(int id, int nuevoValor)
        {
            var dato = db.Tbl_CompraTmp.Where(m => m.id_compraTmp == id).FirstOrDefault();
            dato.cantidad = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/actualizar/descuento/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarDescuento(int id, int nuevoValor)
        {
            var dato = db.Tbl_CompraTmp.Where(m => m.id_compraTmp == id).FirstOrDefault();
            dato.descuento = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/actualizar/precioventa/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarPrecioVenta(int id, int nuevoValor)
        {
            var dato = db.Tbl_CompraTmp.Where(m => m.id_compraTmp == id).FirstOrDefault();
            dato.precioventa = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/actualizar/motivo/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarMotivo(int id, int nuevoValor)
        {
            var dato = db.Tbl_CompraTmp.Where(m => m.id_compraTmp == id).FirstOrDefault();
            dato.motivo = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Facturacion        

        [Route("compras/facturar")]
        [HttpGet]
        public JsonResult Facturar(Tbl_Compra compra)
        {
            bool data = false;

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
                    var proveedor = guardarProveedor("Compra");

                    Tbl_Compra maestro = new Tbl_Compra()
                    {
                        id_proveedor = proveedor,
                        fecha_compra = DateTime.Now,
                        fact_compra = compra.fact_compra,
                        tipo_comprobante_compra = compra.tipo_comprobante_compra,
                        iva_compra = compra.iva_compra,
                        usuario = User.Identity.Name,
                        id_sucursal = (int)suc,
                        estado_compra = true
                    };
                    db.Tbl_Compra.Add(maestro);
                    db.SaveChanges();

                    var detalleTmp = db.Tbl_CompraTmp.Where(m => m.user == User.Identity.Name).ToList();

                    foreach (var articulo in detalleTmp)
                    {
                        var stock = db.Tbl_bodega_productos.Where(m => m.id_sucursal == suc && m.id_articulo == articulo.id_articulo).FirstOrDefault();
                        stock.stock = stock.stock + articulo.cantidad;
                        db.SaveChanges();

                        var cost = articulo.costo;
                        var desc = articulo.descuento;
                        var motivo = "";
                        if (articulo.motivo == 1)
                        {
                            motivo = "Compra";
                        }
                        if (articulo.motivo == 2)
                        {
                            cost = 0;
                            desc = 0;
                            motivo = "Bonificacion";
                        }
                        if (articulo.motivo == 3)
                        {
                            cost = 0;
                            desc = 0;
                            motivo = "Reposicion";
                        }

                        Tbl_Kardex kardex = new Tbl_Kardex()
                        {
                            id_articulo = articulo.id_articulo,
                            fechaKardex = articulo.fecha,
                            num_factura = maestro.fact_compra,
                            Entrada = articulo.cantidad,
                            salida = 0,
                            saldo = stock.stock,
                            ultimoCosto = cost,
                            costoPromedio = cost,
                            usuario = User.Identity.Name,
                            id_sucursal = (int)suc,
                            tipo = "Entrada",
                            observaciones = "Compra Aprovada"
                        };
                        db.Tbl_Kardex.Add(kardex);
                        db.SaveChanges();                       

                        Tbl_Detalle_Compra detalle = new Tbl_Detalle_Compra()
                        {
                            id_compra = maestro.id_compra,
                            id_articulo = articulo.id_articulo,
                            id_Kardex = kardex.id_Kardex,
                            cantidad = articulo.cantidad,
                            costo = cost,
                            descuento = desc,
                            motivo = motivo
                        };
                        db.Tbl_Detalle_Compra.Add(detalle);
                        db.SaveChanges();

                        var precio = db.Tbl_bodega_productos.Where(m => m.id_articulo == articulo.id_articulo).FirstOrDefault();
                        precio.precio = articulo.precioventa;
                        precio.preciocredito = articulo.precioventa * 1.10;
                    }
                    db.Tbl_CompraTmp.RemoveRange(detalleTmp);
                    db.SaveChanges();

                    tran.Commit();
                    data = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }     
            }

            return Json(data , JsonRequestBehavior.AllowGet);
        }

        [Route("compras/cancelar")]
        [HttpPost]
        public JsonResult CancelarCompra()
        {
            var proveedor = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name).ToList();
            var articulos = db.Tbl_CompraTmp.Where(m => m.user == User.Identity.Name).ToList();

            if (proveedor != null)
            {
                db.Tbl_ProveedorTmp.RemoveRange(proveedor);
                db.SaveChanges();
            }
            if (articulos != null)
            {
                db.Tbl_CompraTmp.RemoveRange(articulos);
                db.SaveChanges();
            }
            return Json(Url.Action("Index", "Compras"));
        }

        [Route("compras/detallefactura")]
        public ActionResult Detalle()
        {
            var ultimo = db.Tbl_Compra.Where(m => m.usuario == User.Identity.Name)
                       .OrderByDescending(m => m.id_compra).First();

            var cantidad = db.Tbl_Detalle_Compra.Where(m => m.id_compra == ultimo.id_compra).Sum(m => m.cantidad);
            var descuento = (from v in db.Tbl_Detalle_Compra
                            where v.id_compra == ultimo.id_compra
                            select new
                            {
                                descuento = v.descuento * v.cantidad
                            }).Sum(m => m.descuento);
            var subtotal = (from v in db.Tbl_Detalle_Compra
                            where v.id_compra == ultimo.id_compra
                            select new
                            {
                                sub = (v.costo - v.descuento) * v.cantidad
                            }).Sum(m => m.sub);
            var ivatotal = subtotal * (ultimo.iva_compra / 100);
            var total = subtotal + ivatotal - descuento;
            var id = ultimo.id_compra;

            List<double> datos = new List<double>();
            datos.Add(cantidad);
            datos.Add((float)subtotal);
            datos.Add((float)ivatotal);
            datos.Add((float)descuento);
            datos.Add((float)total);
            datos.Add(id);

            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        public int guardarProveedor(string tipo)
        {
            var nuevo = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).FirstOrDefault();

            if (nuevo.nuevo == true)
            {
                var proveedorTmp = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name).FirstOrDefault();

                Tbl_Proveedor proveedor = new Tbl_Proveedor()
                {
                    razon_social = proveedorTmp.nombre,
                    telefono = proveedorTmp.telefono,
                    Ruc = proveedorTmp.ruc,
                    Estado = true
                };
                db.Tbl_Proveedor.Add(proveedor);
                db.Tbl_ProveedorTmp.Remove(proveedorTmp);
                db.SaveChanges();

                var ultimo = db.Tbl_Proveedor.OrderByDescending(m => m.id_proveedor).FirstOrDefault().id_proveedor;
                return ultimo;
            }
            else
            {
                var proveedorTmp = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name).FirstOrDefault();
                var id = proveedorTmp.id_proveedor;

                var proveedor = db.Tbl_Proveedor.Find(id).id_proveedor;
                db.Tbl_ProveedorTmp.Remove(proveedorTmp);
                db.SaveChanges();

                return proveedor;
            }
        }

        #endregion

        #region ListaCompras

        [Route("compras/listacompras")]
        [HttpGet]
        public JsonResult ListaCompras()
        {
            var compras = (from c in db.Tbl_Compra 
                          where c.usuario == User.Identity.Name
                          select new
                          {
                              ID = c.id_compra,
                              Compra = c.id_compra,
                              Factura = c.fact_compra,
                              Fecha = c.fecha_compra.ToString(),
                              Proveedor = c.Tbl_Proveedor.razon_social,
                              Articulos = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Count(),
                              CantidadTotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.cantidad),
                              PagoTotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.costo) * db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.cantidad),
                              Estado = c.estado_compra
                          }).OrderByDescending(m => m.ID).Take(10).ToList();


            return Json(new { data = compras }, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/detallecompra/general/{id}")]
        [HttpGet]
        public JsonResult DetalleCompraGeneral(int id)
        {
            var detalle = from c in db.Tbl_Compra
                          where c.usuario == User.Identity.Name && c.id_compra == id
                          select new
                          {
                              Usuario = c.usuario,
                              Compra = c.id_compra,
                              Factura = c.fact_compra,
                              Fecha = c.fecha_compra.ToString(),
                              Proveedor = c.Tbl_Proveedor.razon_social,
                              Articulos = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Count(),
                              CantidadTotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.cantidad),
                              Iva = c.iva_compra,
                              DescuentoTotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.descuento),
                              SubTotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.costo),
                              PagoTotal = db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.costo) * db.Tbl_Detalle_Compra.Where(m => m.id_compra == c.id_compra).Sum(m => m.cantidad),
                              Sucursal = c.Tbl_Sucursal.Nombre,
                              Estado = c.estado_compra
                          };

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        }

        [Route("compras/detallecompra/especifico/{id}")]
        [HttpGet]
        public JsonResult DetalleCompraEspecifico(int id)
        {
            var detalle = (from c in db.Tbl_Detalle_Compra
                          where c.id_compra == id
                          select new
                          {
                              Articulo = c.Tbl_Articulo.nombre_articulo,
                              Img = c.Tbl_Articulo.imagen,
                              Categoria = c.Tbl_Articulo.Tbl_Categorias.Nombre,
                              Cantidad = c.cantidad,
                              Descuento = c.descuento,
                              Costo = c.costo,
                              SubTotal = c.cantidad * c.costo,
                              Total = ((c.cantidad * c.costo) - c.descuento )* c.Tbl_Compra.iva_compra
                          }).ToList();

            return Json(new { data = detalle }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}