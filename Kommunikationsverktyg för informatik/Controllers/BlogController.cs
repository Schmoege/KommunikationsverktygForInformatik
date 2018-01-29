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
            List<Post> filteredPosts = new List<Post>();
            if (model.SelectCategories != null)
            {
                var id = context.Categories.Where(x => x.Namn == model.SelectCategories).Select(x => x.Id).First();
                filteredPosts = context.Posts.Where(x => x.KategoriId == id).ToList();
            }
            else
            {
                filteredPosts = blogRepository.GetAll().ToList();
            }
            model.Kategorier = context.Categories.ToList();
            var postCategoriesId = filteredPosts.Select(x => x.Id);
            var categoryFiles = context.UserFiles.Where(x => postCategoriesId.Contains(x.BlogPostId));
            foreach (var post in filteredPosts)
            {
                var newPostFileCombo = new PostFileCombo();
                newPostFileCombo.AttatchedPost = post;
                newPostFileCombo.AttatchedFile = categoryFiles.SingleOrDefault(x => x.BlogPostId == post.Id);
                model.PostFileCombinations.Add(newPostFileCombo);
            }

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
            try
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

                if (model.uploadFile != null)
                {
                    UploadFile(model.uploadFile, model.Post);
                }
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                throw;
            }
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
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase fileToUpload, Post ownerPost)
        {
                var newFile = new UserFile();
                newFile.BlogPost = ownerPost;
                newFile.BlogPostId = ownerPost.Id;
                newFile.FileID = Guid.NewGuid();
                newFile.FileName = fileToUpload.FileName;
                using (var reader = new System.IO.BinaryReader(fileToUpload.InputStream))
                {
                    newFile.FileBytes = reader.ReadBytes(fileToUpload.ContentLength);
                }
                int size = newFile.FileBytes.Length;
                if (size > 15728640)
                {
                    throw new Exception("Den valda filen är för stor. Max storlek är: 15MB. Din storlek: " + size.ToString());
                }
                context.UserFiles.Add(newFile);
                context.SaveChanges();

            

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