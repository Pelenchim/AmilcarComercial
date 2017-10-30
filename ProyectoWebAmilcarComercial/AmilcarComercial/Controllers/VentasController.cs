using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
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

        [Route("ventas/obtener/generales")]
        [HttpGet]
        public JsonResult Generales()
        {
            var venta = (db.Tbl_Compra.OrderByDescending(m => m.id_compra).First().id_compra + 1).ToString();
            var UserLastName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().LastName;
            var UserFirtsName = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().FirstName;

            List<string> lista = new List<string>();
            lista.Add(DateTime.Now.ToString());
            lista.Add((UserFirtsName + ' ' + UserLastName).ToString());
            lista.Add(venta);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        #region Clientes

        [Route("ventas/obtener/clientes")]
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

        [Route("ventas/agregar/clienteTmp")]
        [HttpPost]
        public JsonResult AgregarCliente(Tbl_ClienteTmp cliente)
        {
            cliente.user = User.Identity.Name;

            if (ModelState.IsValid)
            {
                cliente.id_cliente = null;
                cliente.nuevo = true;
                db.Tbl_ClienteTmp.Add(cliente);
                db.SaveChanges();
            }

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("ventas/obtener/clienteTmp")]
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

        [Route("ventas/agregar/clienteExistente/{id}")]
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
            clienteTmp.id_cliente = cliente.id_cliente;

            db.Tbl_ClienteTmp.Add(clienteTmp);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("ventas/eliminar/clienteTmp")]
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

        [Route("ventas/editar/clienteTmp")]
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

        [Route("ventas/editar/clienteGuardarTmp")]
        [HttpPost]
        public JsonResult EditarGuardarClienteTmp(Tbl_ClienteTmp cliente)
        {
            var user = User.Identity.Name;
            cliente.user = user;

            db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Articulos

        [Route("ventas/obtener/articulos")]
        [HttpGet]
        public JsonResult ObtenerArticulos()
        {
            var Articulos = (from p in db.Tbl_Articulo.Where(m => m.estado == true).OrderByDescending(m => m.id_articulo)
                             select new
                             {
                                 ID = p.id_articulo,
                                 Codigo = p.codigo_articulo,
                                 Nombre = p.nombre_articulo,
                                 Imagen = p.imagen,
                                 Stock = p.Tbl_bodega_productos.Where(m => m.id_articulo == p.id_articulo).FirstOrDefault().stock
                             }).ToList();

            return Json(new { data = Articulos }, JsonRequestBehavior.AllowGet);
        }

        [Route("ventas/agregar/producto/{id}/{cant}")]
        [HttpGet]
        public JsonResult AgregarProducto(int id, int cant)
        {
            var user = User.Identity.Name;
            Tbl_OrdenTmp orden = new Tbl_OrdenTmp();

            orden.id_Articulo = id;
            orden.cantidad = cant;
            orden.fecha = DateTime.Now;
            orden.user = user;

            db.Tbl_OrdenTmp.Add(orden);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("ventas/eliminar/productoTmp/{id}")]
        [HttpGet]
        public JsonResult EliminarProducto(int id)
        {
            var articulo = db.Tbl_OrdenTmp.Where(m => m.id_OrdenTmp == id && m.user == User.Identity.Name).FirstOrDefault();
            db.Tbl_OrdenTmp.Remove(articulo);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("ventas/eliminar/eliminarProductosTodos")]
        [HttpGet]
        public JsonResult EliminarProductosTodos()
        {
            var articulos = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name).ToList();
            db.Tbl_OrdenTmp.RemoveRange(articulos);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("ventas/obtener/productosTmp")]
        [HttpGet]
        public JsonResult MostrarProductosTmp()
        {
            if (db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name).Count() != 0)
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
            else
            {
                var datos = 0;

                return Json(datos, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Facturacion

        [Route("ventas/facturar")]
        [HttpGet]
        public JsonResult Facturar(Tbl_Orden venta)
        {
            bool data = false;

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var suc = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Sucursal;
                    var cliente = guardarCliente(); 

                    Tbl_Orden maestro = new Tbl_Orden()
                    {
                        id_sucursal = (int)suc,
                        usuario = User.Identity.Name,
                        fecha_orden = DateTime.Now,
                        iva_orden = venta.iva_orden,
                        estado_orden = "nose",
                        tipo_orden = venta.tipo_orden,
                        fact_Orden = "df",
                        id_cliente = cliente,
                        tipo_pago = venta.tipo_pago
                    };
                    db.Tbl_Orden.Add(maestro);
                    db.SaveChanges();

                    var detalleTmp = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name).ToList();

                    foreach (var articulo in detalleTmp)
                    {
                        Tbl_Kardex kardex = new Tbl_Kardex()
                        {
                            id_articulo = (int)articulo.id_Articulo,
                            fechaKardex = (DateTime)articulo.fecha,
                            num_factura = maestro.fact_Orden,
                            Entrada = 0,
                            salida = (int)articulo.cantidad,
                            saldo = (int)articulo.cantidad,
                            precio = 0,
                            costoPromedio = 0,
                            usuario = User.Identity.Name,
                            id_sucursal = (int)suc
                        };
                        db.Tbl_Kardex.Add(kardex);
                        db.SaveChanges();

                        var stock = db.Tbl_bodega_productos.Where(m => m.id_sucursal == suc && m.id_articulo == articulo.id_Articulo).FirstOrDefault();
                        stock.stock = (int)stock.stock - (int)articulo.cantidad;
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
                    data = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Route("ventas/cancelar")]
        [HttpPost]
        public JsonResult CancelarCompra()
        {
            var cliente = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).ToList();
            var articulos = db.Tbl_OrdenTmp.Where(m => m.user == User.Identity.Name).ToList();

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

            return Json(Url.Action("Index","Ventas"));
        }

        public int guardarCliente()
        {
            var nuevo = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name).FirstOrDefault().nuevo;

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

        [Route("ventas/listaventas")]
        [HttpGet]
        public JsonResult ListaVentas()
        {
            var ventas = (from v in db.Tbl_Orden
                          where v.usuario == User.Identity.Name
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
                              PagoTotal = db.Tbl_Detalle_Orden.Where(m => m.id_orden == v.id_orden).Sum(m => m.precio_venta)
                          }).ToList();

            return Json(new { data = ventas }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}