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
    [Authorize]
    public class MarcasController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();
        UtilitiesController utileria = new UtilitiesController();

        // GET: Marcas
        public ActionResult Index()
        {
            return View(db.Tbl_Marca.Where(m => m.estado == true).ToList());
        }

        public ActionResult Listar()
        {
            return PartialView(db.Tbl_Marca.Where(m => m.estado == true).ToList());
        }

        // GET: Marcas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Marca tbl_Marca = db.Tbl_Marca.Find(id);
            if (tbl_Marca == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Marca);
        }

        // GET: Marcas/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Marcas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Marca,nombre,estado,imagen")] Tbl_Marca tbl_Marca)
        {
            if (ModelState.IsValid)
            {
                var imagen = db.Tbl_ImgTamporal.OrderByDescending(m => m.id_img).Where(m => m.user == User.Identity.Name).FirstOrDefault();
                tbl_Marca.imagen = imagen.imagen;
                var ruta = "marcas";
                utileria.MoverImagen(ruta, imagen.imagen);

                db.Tbl_Marca.Add(tbl_Marca);
                db.Tbl_ImgTamporal.Remove(imagen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Marca);
        }

        // GET: Marcas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Marca tbl_Marca = db.Tbl_Marca.Find(id);
            if (tbl_Marca == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Marca);
        }

        // POST: Marcas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Marca,nombre,estado,imagen")] Tbl_Marca tbl_Marca)
        {
            if (ModelState.IsValid)
            {
                tbl_Marca.estado = false;
                db.Entry(tbl_Marca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Listar");
            }
            return View(tbl_Marca);
        }

        // GET: Marcas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Marca tbl_Marca = db.Tbl_Marca.Find(id);
            if (tbl_Marca == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_Marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Marca tbl_Marca = db.Tbl_Marca.Find(id);
            tbl_Marca.estado = false;
            db.Entry(tbl_Marca).State = EntityState.Modified;
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
