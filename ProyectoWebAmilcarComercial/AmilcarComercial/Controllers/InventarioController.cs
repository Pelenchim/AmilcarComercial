using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
    public class InventarioController : Controller
    {
        DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Inventario
        public ActionResult Index()
        {
            return View();
        }

        [Route("inventario/lista")]
        [HttpGet]
        public JsonResult Lista()
        {
            var user = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Sucursal;

            var lista = (from p in db.Tbl_Articulo where p.Tbl_bodega_productos.FirstOrDefault().id_sucursal == user
                         select new
                         {
                             Id = p.id_articulo,
                             Cod = p.codigo_articulo,
                             Nombre = p.nombre_articulo,
                             Categoria = p.Tbl_Categorias.Nombre,
                             Precio = p.Tbl_Kardex.FirstOrDefault().ultimoCosto,
                             Stock = p.Tbl_bodega_productos.FirstOrDefault().stock
                         }).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}