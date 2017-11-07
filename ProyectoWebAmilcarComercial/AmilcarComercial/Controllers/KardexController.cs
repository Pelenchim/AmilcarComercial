using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
    public class KardexController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Kardex
        public ActionResult Index()
        {
            return View();
        }

        [Route("kardex/lista")]
        [HttpGet]
        public JsonResult Lista()
        {
            var lista = ( from k in db.Tbl_Articulo where k.estado == true
                          select new
                          {
                              ID = k.id_articulo,
                              Cod = k.codigo_articulo,
                              Categoria = k.Tbl_Categorias.Nombre,
                              Articulo = k.nombre_articulo,
                              Existencia = k.Tbl_bodega_productos.Where(m => m.id_articulo == k.id_articulo).FirstOrDefault().stock,
                              Salida = db.Tbl_Kardex.Where(m => m.id_articulo == k.id_articulo && m.salida > 0).OrderByDescending(m => m.fechaKardex).FirstOrDefault().saldo,
                              SalidaFecha = db.Tbl_Kardex.Where(m => m.id_articulo == k.id_articulo && m.salida > 0).FirstOrDefault().fechaKardex.ToString(),
                              Entrada = db.Tbl_Kardex.Where(m => m.id_articulo == k.id_articulo && m.Entrada > 0).OrderByDescending(m => m.fechaKardex).FirstOrDefault().saldo,
                              EntradaFecha = db.Tbl_Kardex.Where(m => m.id_articulo == k.id_articulo && m.Entrada > 0).OrderByDescending(m => m.fechaKardex).FirstOrDefault().fechaKardex.ToString()
                          }).OrderByDescending(m => m.EntradaFecha).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [Route("kardex/detalle/{id}")]
        [HttpGet]
        public JsonResult DetalleKardex(int id)
        {
            var kardex = (from k in db.Tbl_Kardex where k.id_articulo == id
                          select new
                          {
                              Articulo = k.Tbl_Articulo.nombre_articulo,
                              NumFact = k.num_factura,
                              Fecha = k.fechaKardex.ToString(),
                              Entrada = k.Entrada,
                              Salida = k.salida,
                              Nombre = db.AspNetUsers.Where(m => m.UserName == k.usuario).FirstOrDefault().FirstName,
                              Apellido = db.AspNetUsers.Where(m => m.UserName == k.usuario).FirstOrDefault().LastName
                          }).ToList();

            return Json(new { data = kardex }, JsonRequestBehavior.AllowGet);
        }
    }
}