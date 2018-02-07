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
            var Categorias = db.Tbl_Categorias.Include(m => m.Tbl_Categorias2).Where(m => m.estado == true).ToList();

            return View(Categorias);
        }

        public ActionResult Listar()
        {
            var tbl_Categorias = db.Tbl_Categorias.Include(t => t.Tbl_Categorias2).Where(m => m.estado == true);
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
            ViewBag.id_padre = new SelectList(db.Tbl_Categorias.Where(m => m.id_CatPadre == null).ToList(), "Nombre", "id_categoria");

            return PartialView();
        }

        //[HttpPost]
        //[Route("categoria/nueva")]
        public ActionResult Nueva(/*Tbl_Categorias cat, string[] descrip*/)
        {
            //bool data = false;

            //using (var tran = db.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        Tbl_Categorias categoria = new Tbl_Categorias()
            //        {
            //            estado = cat.estado,
            //            Nombre = cat.Nombre,
            //            id_CatPadre = cat.id_CatPadre
            //        };
            //        db.Tbl_Categorias.Add(categoria);
            //        db.SaveChanges();

            //        foreach (var desc in descrip)
            //        {
            //            Tbl_Descripciones descripcion = new Tbl_Descripciones()
            //            {
            //                nombre = desc,
            //                id_categoria = categoria.id_categoria
            //            };
            //            db.Tbl_Descripciones.Add(descripcion);
            //            db.SaveChanges();
            //        }

            //        tran.Commit();
            //        data = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        tran.Rollback();
            //    }
            //}


            //return Json(data, JsonRequestBehavior.AllowGet);
            var cat = db.Tbl_Categorias.Find(1004);
            cat.estado = true;
            db.SaveChanges();

            return RedirectToAction("Index");
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

        public ActionResult Delete(int id)
        {
            Tbl_Categorias cate = db.Tbl_Categorias.Find(id);
            return PartialView(cate);
        } 

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Categorias tbl_Categorias = db.Tbl_Categorias.Find(id);            
            tbl_Categorias.estado = false;
            db.Entry(tbl_Categorias).State = EntityState.Modified;
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
