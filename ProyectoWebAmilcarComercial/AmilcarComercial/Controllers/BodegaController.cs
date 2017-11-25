﻿using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    [Authorize]
    public class BodegaController : Controller
    {
        DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Inventario
        public ActionResult Kardex()
        {
            return View();
        }

        public ActionResult Inventario()
        {
            return View();
        }

        [Route("inventario/lista")]
        [HttpGet]
        public JsonResult ListaInventario()
        {
            var user = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Sucursal;

            var lista = (from p in db.Tbl_Articulo join d in db.Tbl_bodega_productos on p.id_articulo equals d.id_articulo
                         where d.id_sucursal == user
                         select new
                         {
                             Id = p.id_articulo,
                             Cod = p.codigo_articulo,
                             Imagen = p.imagen,
                             Nombre = p.nombre_articulo,
                             Categoria = p.Tbl_Categorias.Nombre,
                             Precio = d.precio,
                             PrecioCredito = d.preciocredito,
                             Stock = d.stock
                         }).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [Route("kardex/lista")]
        [HttpGet]
        public JsonResult ListaKardex()
        {
            var lista = (from p in db.Tbl_Articulo
                         where p.estado == true
                         select new
                         {
                             ID = p.id_articulo,
                             Cod = p.codigo_articulo,
                             Imagen = p.imagen,
                             Categoria = p.Tbl_Categorias.Nombre,
                             Articulo = p.nombre_articulo,
                             Fecha = p.Tbl_Kardex.OrderByDescending(m => m.id_articulo).FirstOrDefault().fechaKardex,
                             SalidaFecha = db.Tbl_Kardex.Where(m => m.id_articulo == p.id_articulo && m.salida > 0).OrderByDescending(m => m.fechaKardex).FirstOrDefault().fechaKardex.ToString(),
                             EntradaFecha = db.Tbl_Kardex.Where(m => m.id_articulo == p.id_articulo && m.Entrada > 0).OrderByDescending(m => m.fechaKardex).FirstOrDefault().fechaKardex.ToString()
                         }).OrderBy(m => m.Fecha).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [Route("kardex/detalle/{id}")]
        [HttpGet]
        public JsonResult DetalleKardex(int id)
        {
            var kardex = (from k in db.Tbl_Kardex
                          where k.id_articulo == id
                          select new
                          {
                              Articulo = k.Tbl_Articulo.nombre_articulo,
                              NumFact = k.num_factura,
                              Fecha = k.fechaKardex.ToString(),
                              Entrada = k.Entrada,
                              Salida = k.salida,
                              Nombre = db.AspNetUsers.Where(m => m.UserName == k.usuario).FirstOrDefault().FirstName,
                              Apellido = db.AspNetUsers.Where(m => m.UserName == k.usuario).FirstOrDefault().LastName,
                              Tipo = k.tipo,
                              Observacion = k.observaciones
                          }).OrderByDescending(m => m.Fecha).ToList();

            return Json(new { data = kardex }, JsonRequestBehavior.AllowGet);
        }
    }
}