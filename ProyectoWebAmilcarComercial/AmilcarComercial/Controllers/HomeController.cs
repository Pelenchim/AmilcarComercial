using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Catalogos()
        {
            return View();
        }
        public ActionResult Configuracion()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("home/user")]
        [HttpGet]
        public JsonResult Usuario()
        {
            var user = (from u in db.AspNetUsers
                        where u.UserName == User.Identity.Name
                        select new
                        {
                            Nombre = u.FirstName,
                            Apellidos = u.LastName,
                            Correo = u.Email,
                            Foto = u.Avatar,
                            Rol = u.AspNetRoles.FirstOrDefault().Name
                        }).FirstOrDefault();

            return Json(user, JsonRequestBehavior.AllowGet);
        }
    }
}