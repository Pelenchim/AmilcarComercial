using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    public class GeneralesController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        #region Proveedores        

        [Route("obtener/proveedores")]
        [HttpGet]
        public JsonResult ObtenerProveedores()
        {
            var Proveedores = (from p in db.Tbl_Proveedor.Where(m => m.Estado == true).OrderByDescending(m => m.id_proveedor)
                               select new
                               {
                                   ID = p.id_proveedor,
                                   Nombre = p.razon_social,
                                   Telefono = p.telefono,
                                   Ruc = p.Ruc
                               }).ToList();

            return Json(new { data = Proveedores }, JsonRequestBehavior.AllowGet);
        }

        [Route("agregar/proveedorTmp/{tipo}")]
        [HttpGet]
        public JsonResult AgregarProveedor(Tbl_ProveedorTmp proveedor, string tipo)
        {
            if (ModelState.IsValid)
            {
                EliminarProveedoresTmp(tipo);

                proveedor.user = User.Identity.Name;
                proveedor.nuevo = true;
                proveedor.id_proveedor = null;
                proveedor.tipo = tipo;
                db.Tbl_ProveedorTmp.Add(proveedor);
                db.SaveChanges();
            }

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/proveedorTmp/{tipo}")]
        [HttpGet]
        public JsonResult MostrarProveedorTmp(Tbl_ClienteTmp cliente, string tipo)
        {
            if (db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).Count() != 0)
            {
                var data = (from p in db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).OrderByDescending(m => m.id_proveedorTmp)
                            select new
                            {
                                Nombre = p.nombre,
                                Telefono = p.telefono,
                                Ruc = p.ruc
                            }).First();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = 0;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("agregar/proveedorExistente/{id}/{tipo}")]
        [HttpGet]
        public JsonResult AgregarProveedorExistente(int id, string tipo)
        {
            var user = User.Identity.Name;
            Tbl_Proveedor proveedor = db.Tbl_Proveedor.Find(id);
            Tbl_ProveedorTmp proveedorTmp = new Tbl_ProveedorTmp();
            EliminarProveedoresTmp(tipo);

            proveedorTmp.user = user;
            proveedorTmp.nombre = proveedor.razon_social;
            proveedorTmp.telefono = proveedor.telefono;
            proveedorTmp.ruc = proveedor.Ruc;
            proveedorTmp.nuevo = false;
            proveedorTmp.tipo = tipo;
            proveedorTmp.id_proveedor = proveedor.id_proveedor;

            db.Tbl_ProveedorTmp.Add(proveedorTmp);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("eliminar/proveedorTmp/{tipo}")]
        public JsonResult EliminarProveedoresTmp(string tipo)
        {
            var lista = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).ToList();

            if (lista != null)
            {
                db.Tbl_ProveedorTmp.RemoveRange(lista);
                db.SaveChanges();
            }
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("editar/proveedorTmp/{tipo}")]
        [HttpGet]
        public JsonResult EditarProveedorTmp(string tipo)
        {
            var proveedor = (from p in db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo)
                             select new
                             {
                                 ID = p.id_proveedorTmp,
                                 Nombre = p.nombre,
                                 Telefono = p.telefono,
                                 Ruc = p.ruc,
                                 tipo = tipo
                             }).FirstOrDefault();
            return Json(proveedor, JsonRequestBehavior.AllowGet);
        }

        [Route("editar/proveedorGuardarTmp/{tipo}")]
        [HttpPost]
        public JsonResult EditarGuardarProveedorTmp(Tbl_ProveedorTmp proveedor, string tipo)
        {
            var nuevo = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).FirstOrDefault().nuevo;

            if (nuevo == true)
            {
                if (ModelState.IsValid)
                {
                    proveedor.user = User.Identity.Name;
                    proveedor.nuevo = true;
                    proveedor.id_proveedor = null;

                    db.Entry(proveedor).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        public int guardarProveedor(string tipo)
        {
            var nuevo = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).FirstOrDefault().nuevo;

            if (nuevo == true)
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

        #region Clientes

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

        [Route("agregar/clienteTmp/{tipo}")]
        [HttpPost]
        public JsonResult AgregarCliente(Tbl_ClienteTmp cliente, string tipo)
        {
            cliente.user = User.Identity.Name;

            if (ModelState.IsValid)
            {
                cliente.id_cliente = null;
                cliente.nuevo = true;
                cliente.tipo = tipo;
                db.Tbl_ClienteTmp.Add(cliente);
                db.SaveChanges();
            }

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/clienteTmp/{tipo}")]
        [HttpGet]
        public JsonResult MostrarClienteTmp(Tbl_ClienteTmp cliente, string tipo)
        {
            if (db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).Count() != 0)
            {
                var data = (from c in db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).OrderByDescending(m => m.id_clienteTmp)
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

        [Route("agregar/clienteExistente/{id}/{tipo}")]
        [HttpGet]
        public JsonResult AgregarClienteExistente(int id, string tipo)
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
            clienteTmp.tipo = tipo;
            clienteTmp.id_cliente = cliente.id_cliente;

            db.Tbl_ClienteTmp.Add(clienteTmp);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("eliminar/clienteTmp/{tipo}")]
        public JsonResult EliminarClientesTmp(string tipo)
        {
            var lista = db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo).ToList();

            if (lista != null)
            {
                db.Tbl_ClienteTmp.RemoveRange(lista);
                db.SaveChanges();
            }
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("editar/clienteTmp/{tipo}")]
        [HttpGet]
        public JsonResult EditarClienteTmp(string tipo)
        {
            var cliente = (from c in db.Tbl_ClienteTmp.Where(m => m.user == User.Identity.Name && m.tipo == tipo)
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

        [Route("editar/clienteGuardarTmp/{tipo}")]
        [HttpPost]
        public JsonResult EditarGuardarClienteTmp(Tbl_ClienteTmp cliente, string tipo)
        {
            var user = User.Identity.Name;
            cliente.user = user;

            db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
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

    }
}