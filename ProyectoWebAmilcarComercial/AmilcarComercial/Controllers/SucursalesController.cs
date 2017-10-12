using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AmilcarComercial.Models;

namespace AmilcarComercial.Controllers
{
    public class SucursalesController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();
        UtilitiesController utileria = new UtilitiesController();

        // GET: Sucursales
        public ActionResult Index()
        {
            ViewBag.Gerente = new SelectList(db.AspNetUsers, "Id", "LastName");
            return View(db.Tbl_Sucursal.Where(m => m.Estado == true).ToList());
        }

        public PartialViewResult Listar()
        {
            ViewBag.Usuarios = new SelectList(db.AspNetUsers, "Id", "LastName");
            return PartialView(db.Tbl_Sucursal.Where(m => m.Estado == true).ToList());
        }

        // GET: Sucursales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Sucursal tbl_Sucursal = db.Tbl_Sucursal.Find(id);
            if (tbl_Sucursal == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_Sucursal);
        }

        // GET: Sucursales/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Sucursales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_sucursal,Nombre,imagen,Estado,Direccion,Telefono,Correo,Gerente")] Tbl_Sucursal tbl_Sucursal)
        {
            if (ModelState.IsValid)
            {
                var imagen = db.Tbl_ImgTamporal.OrderByDescending(m => m.id_img).Where(m => m.user == User.Identity.Name).FirstOrDefault();
                tbl_Sucursal.imagen = imagen.imagen;
                var ruta = "sucursales";
                utileria.MoverImagen(ruta, imagen.imagen);

                db.Tbl_Sucursal.Add(tbl_Sucursal);
                db.Tbl_ImgTamporal.Remove(imagen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index",tbl_Sucursal);
        }

        // GET: Sucursales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Sucursal tbl_Sucursal = db.Tbl_Sucursal.Find(id);
            if (tbl_Sucursal == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_Sucursal);
        }

        // POST: Sucursales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_sucursal,Nombre,imagen,Estado, Direccion,Telefono,Correo,Gerente")] Tbl_Sucursal tbl_Sucursal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Sucursal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Listar");
            }
            return View(tbl_Sucursal);
        }

        // POST: Sucursales/Delete/5
        public ActionResult Delete(int id)
        {
            Tbl_Sucursal tbl_Sucursal = db.Tbl_Sucursal.Find(id);
            db.SaveChanges();
            return PartialView(tbl_Sucursal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Sucursal Tbl_sucursal = db.Tbl_Sucursal.Find(id);
            Tbl_sucursal.Estado = false;
            db.Entry(Tbl_sucursal).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Listar");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
