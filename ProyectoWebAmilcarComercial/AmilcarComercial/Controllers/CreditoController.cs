using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
    public class CreditoController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        #region vistas
        // GET: Credito
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Nueva()
        {
            return View();
        }
        [Route("credito/facturado/{id}")]
        public ActionResult Facturado(string id)
        {
            return View();
        } 
        #endregion

        [Route("credito/obtener/generales")]
        [HttpGet]
        public JsonResult Generales()
        {
            var venta = (db.Tbl_Orden.OrderByDescending(m => m.id_orden).First().id_orden + 1).ToString();
            var UserLastName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().LastName;
            var UserFirtsName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().FirstName;

            List<string> lista = new List<string>();
            lista.Add(DateTime.Now.ToString());
            lista.Add((UserFirtsName + ' ' + UserLastName).ToString());
            lista.Add(venta);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        #region Clientes

        [Route("credito/obtener/clientes")]
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

        [Route("credito/agregar/clienteTmp")]
        [HttpPost]
        public JsonResult AgregarCliente(Tbl_ClienteTmp cliente)
        {
            cliente.user = User.Identity.Name;

            if (ModelState.IsValid)
            {
                cliente.id_cliente = null;
                cliente.nuevo = true;
                cliente.tipoventa = "Credito";
                db.Tbl_ClienteTmp.Add(cliente);
                db.SaveChanges();
            }

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/obtener/clienteTmp")]
        [HttpGet]
        public JsonResult MostrarClienteTmp(Tbl_ClienteTmp cliente)
        {
            if (db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).Count() != 0)
            {
                var data = (from c in db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").OrderByDescending(m => m.id_clienteTmp)
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

        [Route("credito/agregar/clienteExistente/{id}")]
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
            clienteTmp.nuevo = false;
            clienteTmp.tipoventa = "Credito";
            clienteTmp.id_cliente = cliente.id_cliente;

            db.Tbl_ClienteTmp.Add(clienteTmp);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/eliminar/clienteTmp")]
        public JsonResult EliminarClientesTmp()
        {
            var lista = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").ToList();

            if (lista != null)
            {
                db.Tbl_ClienteTmp.RemoveRange(lista);
                db.SaveChanges();
            }
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/editar/clienteTmp")]
        [HttpGet]
        public JsonResult EditarClienteTmp()
        {
            var cliente = (from c in db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito")
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

        [Route("credito/editar/clienteGuardarTmp")]
        [HttpPost]
        public JsonResult EditarGuardarClienteTmp(Tbl_ClienteTmp cliente)
        {
            var user = User.Identity.Name;
            cliente.user = user;

            db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        public int guardarCliente()
        {
            var nuevo = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").FirstOrDefault().nuevo;

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

        #region Articulos

        [Route("credito/obtener/articulos")]
        [HttpGet]
        public JsonResult ObtenerArticulos()
        {
            var articulosOrden = (from p in db.Tbl_OrdenTmp
                                  where (p.user == User.Identity.Name && p.tipoventa == "Credito")
                                  select p.id_Articulo).ToArray();

            var sucursal = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Sucursal;

            var Articulos = (from p in db.Tbl_Articulo join b in db.Tbl_bodega_productos on p.id_articulo equals b.id_articulo
                             where (b.credito == true && b.id_sucursal == sucursal && p.estado == true &&
                                    !(articulosOrden.Contains((int)p.id_articulo)))
                             select new
                             {
                                 ID = p.id_articulo,
                                 Codigo = p.codigo_articulo,
                                 Nombre = p.nombre_articulo,
                                 Imagen = p.imagen,
                                 Stock = b.stock,
                                 Precio = b.preciocredito,
                                 Prima = b.preciocredito * 0.20
                             }).ToList();

            return Json(new { data = Articulos }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/agregar/producto/{id}/{cant}/{meses}/{prima}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id, int cant, int meses, float prima)
        {
            var user = User.Identity.Name;
            Tbl_OrdenTmp orden = new Tbl_OrdenTmp();

            orden.id_Articulo = id;
            orden.cantidad = cant;
            orden.credito_meses = meses;
            orden.prima = prima * cant;
            orden.fecha = DateTime.Now;
            orden.user = user;
            orden.tipoventa = "Credito";

            db.Tbl_OrdenTmp.Add(orden);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/eliminar/productoTmp/{id}")]
        [HttpGet]
        public JsonResult EliminarProducto(int id)
        {
            var articulo = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id && m.user == User.Identity.Name && m.tipoventa == "Credito").FirstOrDefault();
            db.Tbl_OrdenTmp.Remove(articulo);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/eliminar/eliminarProductosTodos")]
        [HttpGet]
        public JsonResult EliminarProductosTodos()
        {
            var articulos = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").ToList();
            db.Tbl_OrdenTmp.RemoveRange(articulos);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/obtener/productosTmp")]
        [HttpGet]
        public JsonResult MostrarProductosTmp()
        {
            if (db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").Count() != 0)
            {
                var fechapago = DateTime.Today.Day;
                var fechafin = DateTime.Today.ToShortDateString();

                var datos = (from p in db.Tbl_OrdenTmp join b in db.Tbl_bodega_productos on p.id_Articulo equals b.id_articulo
                             where (p.user == User.Identity.Name && p.tipoventa == "Credito")
                             select new
                             {
                                 ID = p.id_OrdenTmp,
                                 Nombre = p.Tbl_Articulo.nombre_articulo,
                                 Imagen = p.Tbl_Articulo.imagen,
                                 Cantidad = p.cantidad,
                                 Existecia = b.stock,
                                 Precio = b.preciocredito,
                                 Prima = p.prima,
                                 PrimaMinima = b.preciocredito * 0.20,
                                 Meses = p.credito_meses,
                                 FechaPago = fechapago,
                                 FechaFin = fechafin
                             }).ToList();

                return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var datos = 0;

                return Json(datos, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("credito/actualizar/cantidad/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarCantidad(int id, int nuevoValor)
        {
            var dato = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id && m.tipoventa == "Credito").FirstOrDefault();
            dato.cantidad = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/actualizar/prima/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarPrima(int id, int nuevoValor)
        {
            var dato = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id && m.tipoventa == "Credito").FirstOrDefault();
            dato.prima = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/actualizar/meses/productoTmp/{id}/{nuevoValor}")]
        [HttpGet]
        public JsonResult ActualizarMeses(int id, int nuevoValor)
        {
            var dato = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id && m.tipoventa == "Credito").FirstOrDefault();
            dato.credito_meses = nuevoValor;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Facturacion

        [Route("credito/facturar")]
        [HttpGet]
        public JsonResult Facturar(Tbl_Orden venta)
        {
            string[] valores = new string[2];
            valores[0] = "false";

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
                    var fecha = DateTime.Now.Date;
                    var cliente = guardarCliente();

                    Tbl_Orden maestro = new Tbl_Orden()
                    {
                        id_sucursal = (int)suc,
                        usuario = User.Identity.Name,
                        fecha_orden = fecha,
                        iva_orden = venta.iva_orden,
                        estado = true,
                        tipo_orden = "Credito",
                        fact_Orden = venta.fact_Orden,
                        id_cliente = cliente,
                        tipo_pago = venta.tipo_pago
                    };
                    db.Tbl_Orden.Add(maestro);
                    db.SaveChanges();

                    var detalleTmp = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").ToList();

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
                            id_sucursal = (int)suc
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
                    valores[0] = "true";
                    valores[1] = (maestro.id_orden + 1).ToString();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            return Json(new { data = valores }, JsonRequestBehavior.AllowGet);
        }

        [Route("credito/cancelar")]
        [HttpPost]
        public JsonResult CancelarCompra()
        {
            var cliente = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").ToList();
            var articulos = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name && m.tipoventa == "Credito").ToList();

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

            return Json(Url.Action("Index", "Credito"));
        }

        #endregion

        [Route("credito/listaventas")]
        [HttpGet]
        public JsonResult ListaVentas()
        {
            var ventas = (from v in db.Tbl_Orden
                          where v.usuario == User.Identity.Name && v.tipo_orden == "Credito"
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
    }
}