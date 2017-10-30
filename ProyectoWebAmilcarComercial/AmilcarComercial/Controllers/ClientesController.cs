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
    public class ClientesController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Clientes
        public ActionResult Index()
        {
            var tbl_Clientes = db.Tbl_Clientes.Include(t => t.Tbl_Departamentos).Where(m => m.estado == true).ToList();
            return View(tbl_Clientes);
        }

        public ActionResult Listar()
        {
            var tbl_Clientes = db.Tbl_Clientes.Include(t => t.Tbl_Departamentos).Where(m => m.estado == true).ToList();
            return PartialView(tbl_Clientes);
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Clientes tbl_Clientes = db.Tbl_Clientes.Find(id);
            if (tbl_Clientes == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_Clientes);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.departamento = new SelectList(db.Tbl_Departamentos, "id_Departamento", "Nombre");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_cliente,nombre_cliente,apellidos_cliente,direccion,departamento,telefono,cedula,estado")] Tbl_Clientes tbl_Clientes)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Clientes.Add(tbl_Clientes);
                db.SaveChanges();
                return RedirectToAction("Listar");
            }

            ViewBag.departamento = new SelectList(db.Tbl_Departamentos, "id_Departamento", "Nombre", tbl_Clientes.departamento);
            return View(tbl_Clientes);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Clientes tbl_Clientes = db.Tbl_Clientes.Find(id);
            if (tbl_Clientes == null)
            {
                return HttpNotFound();
            }
            ViewBag.departamento = new SelectList(db.Tbl_Departamentos, "id_Departamento", "Nombre", tbl_Clientes.departamento);
            return View(tbl_Clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_cliente,nombre_cliente,apellidos_cliente,direccion,departamento,telefono,cedula,estado")] Tbl_Clientes tbl_Clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Listar");
            }
            ViewBag.departamento = new SelectList(db.Tbl_Departamentos, "id_Departamento", "Nombre", tbl_Clientes.departamento);
            return View(tbl_Clientes);
        }

        public ActionResult Delete(int id)
        {
            Tbl_Clientes tbl_Clientes = db.Tbl_Clientes.Find(id);
            return PartialView(tbl_Clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Clientes tbl_Clientes = db.Tbl_Clientes.Find(id);
            tbl_Clientes.estado = false;
            db.Entry(tbl_Clientes).State = EntityState.Modified;
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
