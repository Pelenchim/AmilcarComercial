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

        // GET: Sucursales
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Listar()
        {
            return PartialView(db.Tbl_Sucursal.ToList());
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
            return View(tbl_Sucursal);
        }

        // GET: Sucursales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sucursales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_sucursal,Nombre,imagen,Estado")] Tbl_Sucursal tbl_Sucursal)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Sucursal.Add(tbl_Sucursal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Sucursal);
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
            return View(tbl_Sucursal);
        }

        // POST: Sucursales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_sucursal,Nombre,imagen,Estado")] Tbl_Sucursal tbl_Sucursal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Sucursal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Sucursal);
        }

        // GET: Sucursales/Delete/5
        public ActionResult Delete(int? id)
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
            return View(tbl_Sucursal);
        }

        // POST: Sucursales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Sucursal tbl_Sucursal = db.Tbl_Sucursal.Find(id);
            db.Tbl_Sucursal.Remove(tbl_Sucursal);
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
