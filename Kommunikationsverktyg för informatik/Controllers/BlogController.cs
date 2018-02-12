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

        public ActionResult Index(BlogPostViewModel model, bool formal = false, bool hidden = false, int newCount = 5, int oldCount = 5)
        {
            model.Formal = formal;
            blogRepository.ClearUnreadPosts(User.Identity.GetUserId());
            List<Post> filteredPosts = new List<Post>();
            if (model.SelectedCategory != null)
            {
                var id = context.Categories.Where(x => x.Namn == model.SelectedCategory).Select(x => x.Id).First();
                filteredPosts = context.Posts.Where(x => x.KategoriId == id && x.Hidden == false).Take(newCount).ToList();
                model.DbCount = context.Posts.Where(x => x.KategoriId == id).Count();
                model.NewCount = filteredPosts.Count;
            }
            else
            {
                filteredPosts = blogRepository.GetAll(formal).Take(newCount).ToList();
                model.DbCount = blogRepository.GetAll(formal).Count();
                model.NewCount = filteredPosts.Count;
            }
            model.OldCount = oldCount;
            model.Kategorier = context.Categories.Where(x => x.Formell == formal).ToList();
            var postCategoriesId = filteredPosts.Select(x => x.Id);
            var categoryFiles = context.UserFiles.Where(x => postCategoriesId.Contains(x.BlogPostId)).ToList();
            var picExtensionList = new List<string>() {".PNG",".JPG",".JPEG" };
            var picList = categoryFiles.Where(x => picExtensionList.Contains(x.FileExtension.ToUpper()));
            var userIdList = filteredPosts.Select(x => x.User).ToList();
            IEnumerable<ApplicationUser> users = context.Users.Where(x => userIdList.Contains(x.Id));
            if(hidden)
            {
                string myID = User.Identity.GetUserId();
                model.Hidden = true;
                filteredPosts = context.Posts.Where(x => x.Hidden == true && x.User == myID).ToList();
            }
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
        public ActionResult Create(bool formal = false)
        {
            BlogPostViewModel model = new BlogPostViewModel()
            {
                Post = new Post()
            };
            model.Formal = formal;
            model.Kategorier = context.Categories.Where(x => x.Formell == formal).ToList();
            return View(model);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        [HttpPost]
        public ActionResult Create(BlogPostViewModel model, FormCollection collection)
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
                model.Location.Latitude = collection["lat"];
                model.Location.Longitude = collection["long"];
                model.Location.PostId = model.Post.Id;
                context.Location.Add(model.Location);
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
                return RedirectToAction("Index", new { formal = model.Formal });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }
           return RedirectToAction("Index", new { formal = model.Formal });
        }

        [Authorize]
        public ActionResult CreateKategori(bool formal = false)
        {
            ViewBag.Formal = formal;
            return View();
        }

        [HttpPost]
        public ActionResult CreateKategori(Kategori kategori, bool formal = false)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            context.Categories.Add(kategori);
            context.SaveChanges();
            return RedirectToAction("Create", new { formal = formal });
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
            postToEdit.Hidden = postInfo.Hidden;
            context.Entry(postToEdit).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult BlogCancel (Guid id)
        {
            
            
            Post postDelete = context.Posts.Find(id);
            
            Location locationDelete = context.Location.Find(id);
            IEnumerable<Comment> relatedComments = context.Comments
                .Where(x => x.PostID == id);

            context.Comments.RemoveRange(relatedComments);
            context.Location.Remove(locationDelete);
            context.Posts.Remove(postDelete);
            
            context.SaveChanges();
            
            return RedirectToAction("Index");
            
        }
        
        public ActionResult MyHiddenPosts()
        {
            return RedirectToAction("Index", new { hidden = true });
        }

    }
}