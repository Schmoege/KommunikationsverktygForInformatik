using DataAccess.Models;
using DataAccess.Repositories;
using Kommunikationsverktyg_för_informatik.Models;
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

        public ActionResult Index(BlogPostViewModel model, int newCount = 5, int oldCount = 5)
        {
            blogRepository.ClearUnreadPosts(User.Identity.GetUserId());
            List<Post> filteredPosts = new List<Post>();
            if (model.SelectedCategory != null)
            {
                var id = context.Categories.Where(x => x.Namn == model.SelectedCategory).Select(x => x.Id).First();
                filteredPosts = context.Posts.Where(x => x.KategoriId == id).Take(newCount).ToList();
                model.DbCount = context.Posts.Where(x => x.KategoriId == id).Count();
                model.NewCount = filteredPosts.Count;
            }
            else
            {
                filteredPosts = blogRepository.GetAll().Take(newCount).ToList();
                model.DbCount = blogRepository.GetAll().Count();
                model.NewCount = filteredPosts.Count;
            }
            model.OldCount = oldCount;
            model.Kategorier = context.Categories.ToList();
            var postCategoriesId = filteredPosts.Select(x => x.Id);
            var categoryFiles = context.UserFiles.Where(x => postCategoriesId.Contains(x.BlogPostId)).ToList();
            var picExtensionList = new List<string>() {".png",".PNG", ".jpg",".JPG",".jpeg",".JPEG" };
            var picList = categoryFiles.Where(x => picExtensionList.Contains(x.FileExtension));
            var userIdList = filteredPosts.Select(x => x.User).ToList();
            IEnumerable<ApplicationUser> users = context.Users.Where(x => userIdList.Contains(x.Id));
            foreach (var post in filteredPosts)
            {
                var newPostFileCombo = new PostFileCombo();
                newPostFileCombo.AttatchedPost = post;

                newPostFileCombo.AttachedUser = users.SingleOrDefault(x => x.Id == post.User);

                var postPics = picList.Where(x => x.BlogPostId.Equals(post.Id));

                newPostFileCombo.AttatchedPics = postPics.ToList();
                newPostFileCombo.AttatchedDocs = categoryFiles.Where(x => x.BlogPostId == post.Id).Where(x => !picExtensionList.Contains(x.FileExtension)).ToList();
                model.PostFileCombinations.Add(newPostFileCombo);
            }
            return View(model);

        }
        [HttpPost]
        public ActionResult LoadPosts(int count = 5)
        {
            return RedirectToAction("Index", new { oldCount = count, newCount = count + 5 });
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
                if (!ModelState.IsValid || model.SelectedCategory == null)
                {
                    model.Kategorier = context.Categories.ToList();
                    ViewBag.CategoryError = "Du måste skapa en kategori för att kunna lägga upp ett inlägg";
                    return View(model);
                }
                else
                {
                    ViewBag.CategoryError = ""; //Om du lägger till kategori men inte ngn fil så ska du inte få fel här
                }
                model.Post.Date = DateTime.Now;
                model.Post.User = User.Identity.GetUserId(); 
                model.Post.KategoriId = context.Categories
                    .Where(x => x.Namn == model.SelectedCategory)
                    .Select(x => x.Id).First();
                blogRepository.AddPost(model.Post);
                //context.Posts.Add(model.Post);

                if (model.uploadFiles[0] != null) //Den skickar alltid med någon jävel
                {
                    foreach (var file in model.uploadFiles) //Kontrollera så att alla filer är under 15MB
                    {
                        if (file.ContentLength > 15728640) //15MB i Bytes
                        {
                            model.Kategorier = context.Categories.ToList();
                            ViewBag.FileError = file.FileName + " är för stor. Storlek: " + (file.ContentLength / 1024).ToString() + "KB"; ;
                            return View(model);
                        }
                        
                    }
                    UploadFile(model.uploadFiles, model.Post);

                }

                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
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
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase[] filesToUpload, Post ownerPost)
        {
            foreach (var fileToUpload in filesToUpload)
            {
                var newFile = new UserFile();
                newFile.BlogPostId = ownerPost.Id;
                newFile.FileID = Guid.NewGuid();
                newFile.FileName = fileToUpload.FileName;
                newFile.FileExtension = System.IO.Path.GetExtension(fileToUpload.FileName).ToString();
                using (var reader = new System.IO.BinaryReader(fileToUpload.InputStream))
                {
                    newFile.FileBytes = reader.ReadBytes(fileToUpload.ContentLength);
                }
                context.UserFiles.Add(newFile);
            }
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

        public ActionResult Edit(Guid Id)
        {
            Post post = context.Posts.SingleOrDefault(x => x.Id == Id);

            return View(post);
        }
        [HttpPost]
        public ActionResult Edit(Post postInfo)
        {
            Post postToEdit = context.Posts.SingleOrDefault(x => x.Id == postInfo.Id);
            postToEdit.Title = postInfo.Title;
            postToEdit.Description = postInfo.Description;
            context.Entry(postToEdit).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult BlogCancel (Guid id)
        {
            Post postDelete = context.Posts.Find(id);
            context.Posts.Remove(postDelete);
            context.SaveChanges();
            return RedirectToAction("Index");


           
        }

    }
}