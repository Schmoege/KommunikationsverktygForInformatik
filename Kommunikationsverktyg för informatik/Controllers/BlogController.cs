using DataAccess.Models;
using DataAccess.Repositories;
using Kommunikationsverktyg_för_informatik.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class BlogController : Controller
    {
        DataContext context = new DataContext();

        private BlogRepository blogRepository;
        public BlogController()
        {
            DataContext dataContext = new DataContext();
            blogRepository = new BlogRepository(dataContext);
        }



        // GET: Blog
        [Authorize]

        public ActionResult Index(BlogPostViewModel model)
        {
            if (model.SelectCategories != null)
            {
                var id = context.Categories.Where(x => x.Namn == model.SelectCategories).Select(x => x.Id).First();
                model.Posts = context.Posts.Where(x => x.KategoriId == id).ToList();
            }
            else
            {
                model.Posts = blogRepository.GetAll().ToList();
            }
            model.Kategorier = context.Categories.ToList();
            return View(model);

        }


        [Authorize]
        public ActionResult Create()
        {
            BlogPostViewModel model = new BlogPostViewModel();
            model.Kategorier = context.Categories.ToList();
            return View(model);

        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(BlogPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            model.Post.Date = DateTime.Now;
            model.Post.UserName = User.Identity.GetUserName();
            model.Post.KategoriId = context.Categories
                .Where(x => x.Namn == model.SelectCategories)
                .Select(x => x.Id).First();
            context.Posts.Add(model.Post);
            context.SaveChanges();
            if (model.uploadFile != null)
            {
                UploadFile(model.uploadFile, model.Post);
            }
            
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult CreateKategori()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateKategori(Kategori kategori)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            context.Categories.Add(kategori);
            context.SaveChanges();
            return RedirectToAction("Create");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase fileToUpload, Post ownerPost)
        {
            try
            {
                
                var newFile = new UserFile();
                newFile.BlogPost = ownerPost; //Ändra till postobjekt senare
                newFile.FileID = Guid.NewGuid();
                newFile.FileName = fileToUpload.FileName;
                using (var reader = new System.IO.BinaryReader(fileToUpload.InputStream))
                {
                    newFile.FileBytes = reader.ReadBytes(fileToUpload.ContentLength);
                }
                context.UserFiles.Add(newFile);
                context.SaveChanges();

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