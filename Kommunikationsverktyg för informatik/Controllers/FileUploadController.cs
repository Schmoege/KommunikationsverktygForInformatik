using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kommunikationsverktyg_för_informatik.Controllers
{

    
    public class FileUploadController : Controller
    {
        
        public ActionResult FileUpload()
        {
            List<UserFile> files;
            using (var db = new DataContext())
            {
                files = db.UserFiles.ToList();
            }
            return View(files);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase fileToUpload)
        {
            try
            {
                if (fileToUpload == null)
                {
                    RedirectToAction("Index", "Home");
                }
                var ownerPost = new Post();

                var newFile = new UserFile();
                newFile.BlogPost = null; //Ändra till postobjekt senare
                newFile.FileID = Guid.NewGuid();
                newFile.FileType = Path.GetExtension(fileToUpload.FileName);
                newFile.FileName = fileToUpload.FileName;
                using (var reader = new BinaryReader(fileToUpload.InputStream))
                {
                    newFile.FileBytes = reader.ReadBytes(fileToUpload.ContentLength);
                }
                using (var dbContext = new DataContext())
                {
                    dbContext.UserFiles.Add(newFile);
                    dbContext.SaveChanges();
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                throw;
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DownloadFile(string downloadFileId)
        {
            UserFile fileToDownload;
            using (var db = new DataContext())
            {
                fileToDownload = db.UserFiles.Single(x => x.FileID.Equals(new Guid(downloadFileId)));
            }
            return File(fileToDownload.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileToDownload.FileName);
        }
    }
}