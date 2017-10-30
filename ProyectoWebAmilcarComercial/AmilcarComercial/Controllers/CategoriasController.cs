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
    public class CategoriasController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Categorias
        public ActionResult Index()
        {
            var tbl_Categorias = db.Tbl_Categorias.Include(t => t.Tbl_Categorias2);
            return View(tbl_Categorias.ToList());
        }

        public ActionResult Listar()
        {
            var tbl_Categorias = db.Tbl_Categorias.Include(t => t.Tbl_Categorias2);
            return PartialView(tbl_Categorias.ToList());
        }

        // GET: Categorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Categorias tbl_Categorias = db.Tbl_Categorias.Find(id);
            if (tbl_Categorias == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_Categorias);
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            ViewBag.id_CatPadre = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre");
            return PartialView();
        }

        // POST: Categorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_categoria,Nombre,id_CatPadre,estado")] Tbl_Categorias tbl_Categorias)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Categorias.Add(tbl_Categorias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_CatPadre = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre", tbl_Categorias.id_CatPadre);
            return PartialView(tbl_Categorias);
        }

        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Categorias tbl_Categorias = db.Tbl_Categorias.Find(id);
            if (tbl_Categorias == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_CatPadre = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre", tbl_Categorias.id_CatPadre);
            return PartialView(tbl_Categorias);
        }

        // POST: Categorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_categoria,Nombre,id_CatPadre,estado")] Tbl_Categorias tbl_Categorias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Categorias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_CatPadre = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre", tbl_Categorias.id_CatPadre);
            return PartialView(tbl_Categorias);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Categorias tbl_Categorias = db.Tbl_Categorias.Find(id);
            db.Tbl_Categorias.Remove(tbl_Categorias);
            db.SaveChanges();
            return RedirectToAction("Index");
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
