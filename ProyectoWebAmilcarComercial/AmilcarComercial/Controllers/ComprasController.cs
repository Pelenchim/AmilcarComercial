using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    public class ComprasController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Compras
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nueva()
        {
            return View();
        }

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

        [Route("agregar/proveedorTmp")]
        [HttpPost]
        public JsonResult AgregarProveedor(Tbl_ProveedorTmp proveedor)
        {
            proveedor.user = User.Identity.Name;

            if (ModelState.IsValid)
            {
                proveedor.nuevo = true;
                db.Tbl_ProveedorTmp.Add(proveedor);
                db.SaveChanges();
            }

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/proveedorTmp")]
        [HttpGet]
        public JsonResult MostrarProveedorTmp(Tbl_ClienteTmp cliente)
        {
            if (db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name).Count() != 0)
            {
                var data = (from p in db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name).OrderByDescending(m => m.id_proveedorTmp)
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

        [Route("agregar/proveedorExistente/{id}")]
        [HttpGet]
        public JsonResult AgregarProveedorExistente(int id)
        {
            var user = User.Identity.Name;
            Tbl_Proveedor proveedor = db.Tbl_Proveedor.Find(id);
            Tbl_ProveedorTmp proveedorTmp = new Tbl_ProveedorTmp();
            EliminarProveedoresTmp();

            proveedorTmp.user = user;
            proveedorTmp.nombre = proveedor.razon_social;
            proveedorTmp.telefono = proveedor.telefono;
            proveedorTmp.ruc = proveedor.Ruc;
            proveedorTmp.nuevo = false;

            db.Tbl_ProveedorTmp.Add(proveedorTmp);
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("eliminar/proveedorTmp")]
        public JsonResult EliminarProveedoresTmp()
        {
            var lista = db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name).ToList();

            if (lista != null)
            {
                db.Tbl_ProveedorTmp.RemoveRange(lista);
                db.SaveChanges();
            }
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        [Route("editar/proveedorTmp")]
        [HttpGet]
        public JsonResult EditarProveedorTmp()
        {
            var proveedor = (from p in db.Tbl_ProveedorTmp.Where(m => m.user == User.Identity.Name)
                           select new
                           {
                               ID = p.id_proveedorTmp,
                               Nombre = p.nombre,
                               Telefono = p.telefono,
                               Ruc = p.ruc
                           }).FirstOrDefault();
            return Json(new { data = proveedor }, JsonRequestBehavior.AllowGet);
        }

        [Route("editar/proveedorGuardarTmp")]
        [HttpPost]
        public JsonResult EditarGuardarProveedorTmp(Tbl_ProveedorTmp proveedor)
        {
            var user = User.Identity.Name;
            proveedor.user = user;
            proveedor.nuevo = true;

            db.Entry(proveedor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }
    }
}