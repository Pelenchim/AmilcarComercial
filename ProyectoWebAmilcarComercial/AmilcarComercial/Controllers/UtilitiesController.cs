using AmilcarComercial.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmilcarComercial.Controllers
{
    public class UtilitiesController : Controller
    {
        private DBAmilcarEntities db = new DBAmilcarEntities();

        // GET: Utilities
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("Utilities/SaveUploadedFile/{id}")]
        public ActionResult SaveUploadedFile(int id)
        {
            var Ultimo = "";

            if (id == 0)
            {
                Ultimo = "art" + db.Tbl_Articulo.OrderByDescending(m => m.id_articulo).FirstOrDefault().id_articulo + "-";
            }
            else if (id == 1)
            {
                Ultimo = "mar" + db.Tbl_Marca.OrderByDescending(m => m.id_Marca).FirstOrDefault().id_Marca + "-";
            }
            else if (id == 2)
            {
                Ultimo = "suc" + db.Tbl_Sucursal.OrderByDescending(m => m.id_sucursal).FirstOrDefault().id_sucursal + "-";
            }

            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Content\\imagesTemporales", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString());

                        var fileName1 = string.Format(Ultimo + System.DateTime.Now.ToString("MMddyyyyHHmm") + Path.GetExtension(file.FileName));

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, fileName1);
                        file.SaveAs(path);

                        Tbl_ImgTamporal imagen = new Tbl_ImgTamporal();
                        imagen.imagen = fileName1;
                        imagen.user = User.Identity.Name;
                        db.Tbl_ImgTamporal.Add(imagen);
                        db.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        public ActionResult GetAttachments()
        {
            //Get the images list from repository
            var attachmentsList = new List<UtilitiesModel>
            {
                new UtilitiesModel {AttachmentID = 1, FileName = "/images/wallimages/dropzonelayout.png", Path = "/images/wallimages/dropzonelayout.png"},
                new UtilitiesModel {AttachmentID = 1, FileName = "/images/wallimages/imageslider-3.png", Path = "/images/wallimages/imageslider-3.png"}
            }.ToList();

            return Json(new { Data = attachmentsList }, JsonRequestBehavior.AllowGet);
        }

        public void MoverImagen(string ruta, string imagen)
        {
            string fileName = imagen;
            string sourcePath =  @"C:\Users\HP I7\Documents\GitHub\SistemaWeb\ProyectoWebAmilcarComercial\AmilcarComercial\Content\imagesTemporales";
            string targetPath = @"C:\Users\HP I7\Documents\GitHub\SistemaWeb\ProyectoWebAmilcarComercial\AmilcarComercial\Content\images\" + ruta;

            if (System.IO.File.Exists(string.Format(sourcePath + "\\" + fileName)))
            {
                try
                {
                    // Use Path class to manipulate file and directory paths.
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);

                    // To copy a folder's contents to a new location:
                    // Create a new target folder, if necessary.
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }

                    // To copy a file to another location and 
                    // overwrite the destination file if it already exists.
                    System.IO.File.Copy(sourceFile, destFile, true);

                    // Eliminando la imagen de la carpeta imagesTemporales
                    System.IO.File.Delete(string.Format(sourcePath + "\\" + fileName));
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }            
        }
    }
}