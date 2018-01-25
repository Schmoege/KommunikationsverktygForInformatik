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
                    newFile.Path = reader.ReadBytes(fileToUpload.ContentLength);
                }
                using (var dbContext = new DataContext())
                {
                    dbContext.UserFiles.Add(newFile);
                    dbContext.SaveChanges();
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write("Hello via Debug!");
                System.Diagnostics.Debug.Write(e.Message);
                System.Diagnostics.Debug.Write( "Hello via Debug!" );
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
            byte[] fileBytes = GetFile(fileToDownload);
            var fileName = fileToDownload.FileName.Split(Char.Parse("."))[0];
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        private byte[] GetFile(UserFile userFile)
        {
            var fullName = userFile.FileName;
            System.IO.FileStream fileRead = System.IO.File.OpenRead(fullName);

            byte[] data = new byte[fileRead.Length];
            //int returnData = fileRead.Read(data, 0, data.Length);
            return data;
        }
        //public ActionResult Download(string filePath, string fileName)
        //{
        //    string fullName = Path.Combine(GetBaseDir(), filePath, fileName);

        //    byte[] fileBytes = GetFile(fullName);
        //    return File(
        //        fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}

        //byte[] GetFile(string s)
        //{
        //    System.IO.FileStream fs = System.IO.File.OpenRead(s);
        //    byte[] data = new byte[fs.Length];
        //    int br = fs.Read(data, 0, data.Length);
        //    if (br != fs.Length)
        //        throw new System.IO.IOException(s);
        //    return data;
        //}
    }
}