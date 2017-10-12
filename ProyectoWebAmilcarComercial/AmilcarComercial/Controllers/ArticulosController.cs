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
    public class ArticulosController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();
        UtilitiesController utileria = new UtilitiesController();

        // GET: Articulos
        public ActionResult Index()
        {
            ViewBag.id_categoria = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre");
            ViewBag.id_marca = new SelectList(db.Tbl_Marca, "id_Marca", "nombre");
            var tbl_Articulo = db.Tbl_Articulo.Where(m => m.estado == true).Include(t => t.Tbl_Categorias).Include(t => t.Tbl_Marca);
            return View(tbl_Articulo.ToList());
        }

        public ActionResult Listar()
        {
            ViewBag.id_categoria = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre");
            ViewBag.id_marca = new SelectList(db.Tbl_Marca, "id_Marca", "nombre");
            var tbl_Articulo = db.Tbl_Articulo.Where(m => m.estado == true).Include(t => t.Tbl_Categorias).Include(t => t.Tbl_Marca);
            return PartialView(tbl_Articulo.ToList());
        }

        // GET: Articulos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Articulo tbl_Articulo = db.Tbl_Articulo.Find(id);
            if (tbl_Articulo == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_Articulo);
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_articulo,codigo_articulo,nombre_articulo,descripcion_articulo,id_categoria,credito_articulo,id_Marca,GarantiaCant,GarantiaMedida")] Tbl_Articulo tbl_Articulo)
        {
            if (ModelState.IsValid)
            {
                var imagen = db.Tbl_ImgTamporal.OrderByDescending(m => m.id_img).Where(m => m.user == User.Identity.Name).FirstOrDefault();
                tbl_Articulo.imagen = imagen.imagen;
                var ruta = "articulos";
                utileria.MoverImagen(ruta, imagen.imagen);

                db.Tbl_Articulo.Add(tbl_Articulo);
                db.Tbl_ImgTamporal.Remove(imagen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_categoria = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre", tbl_Articulo.id_categoria);
            ViewBag.id_categoria = new SelectList(db.Tbl_Marca, "id_Marca", "nombre", tbl_Articulo.id_categoria);
            return RedirectToAction("Index",tbl_Articulo);
        }

        // GET: Articulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Articulo tbl_Articulo = db.Tbl_Articulo.Find(id);
            if (tbl_Articulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre", tbl_Articulo.id_categoria);
            ViewBag.id_categoria = new SelectList(db.Tbl_Marca, "id_Marca", "nombre", tbl_Articulo.id_categoria);
            return View(tbl_Articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_articulo,codigo_articulo,nombre_articulo,descripcion_articulo,imagen,id_categoria,credito_articulo,id_Marca,GarantiaCant,GarantiaMedida")] Tbl_Articulo tbl_Articulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Articulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Listar");
            }
            ViewBag.id_categoria = new SelectList(db.Tbl_Categorias, "id_categoria", "Nombre", tbl_Articulo.id_categoria);
            ViewBag.id_categoria = new SelectList(db.Tbl_Marca, "id_Marca", "nombre", tbl_Articulo.id_categoria);
            return View(tbl_Articulo);
        }

        // GET: Articulos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Articulo tbl_Articulo = db.Tbl_Articulo.Find(id);
            if (tbl_Articulo == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_Articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Articulo tbl_Articulo = db.Tbl_Articulo.Find(id);
            tbl_Articulo.estado = false;
            db.Entry(tbl_Articulo).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Listar");
        }

        [Route("obtener/especificaciones/{id}")]
        [HttpGet]
        public JsonResult ObtenerEspecificacionesCategoria(int id)
        {
            //Busca las especificaciones de la categoria especifica
            var Especificaciones = (from p in db.Tbl_Descripciones
                                    where p.id_categoria == id
                                    select new
                                    {
                                        ID = p.id_descripcion,
                                        Nombre = p.nombre
                                    }).ToList();

            return Json(new { data = Especificaciones }, JsonRequestBehavior.AllowGet);
        }

        [Route("obtener/ultimo")]
        [HttpGet]
        public JsonResult ObtenerUltimoArticulo()
        {
            //Busca las especificaciones de la categoria especifica
            var Ultimo = (from p in db.Tbl_Articulo.OrderByDescending(m => m.id_articulo)
                          select new
                          {
                              ID = "art" + p.id_articulo
                          }).FirstOrDefault();

            return Json(new { data = Ultimo }, JsonRequestBehavior.AllowGet);
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
