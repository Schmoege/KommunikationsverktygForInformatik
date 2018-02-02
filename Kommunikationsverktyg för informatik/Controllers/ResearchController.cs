using Kommunikationsverktyg_för_informatik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Repositories;
using Microsoft.AspNet.Identity;
using DataAccess.Models;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class ResearchController : Controller
    {
        ResearchRepository rr = new ResearchRepository();
        // GET: Research
        [Authorize]
        public ActionResult Index()
        {
            

            ResearchBlogViewModel model = new ResearchBlogViewModel();
            model.Posts = rr.GetAll().ToList();
            

            foreach(var post in model.Posts)
            {
                ResearchPostFileCombo rpfc = new ResearchPostFileCombo()
                {
                    AttatchedPost = post
                };
                using (DataContext context = new DataContext())
                {
                    ResearchPost rp = new ResearchPost();
                    var ResearchPostId = context.ResearchPosts.Select(r => r.Id);
                    var categoryFiles = context.UserFiles.Where(x => ResearchPostId.Contains(x.BlogPostId)).ToList();
                    var picExtensionList = new List<string>() { ".png", ".PNG", ".jpg", ".JPG", ".jpeg", ".JPEG" };
                    var picList = categoryFiles.Where(x => picExtensionList.Contains(x.FileExtension));
                    rpfc.AttatchedPics = picList.ToList();
                    rpfc.AttatchedDocs = categoryFiles.Where(x => x.BlogPostId == post.Id).Where(x => !picExtensionList.Contains(x.FileExtension)).ToList();


                }
                model.PostFileCombinations.Add(rpfc);
            }
            
                return View(model);
        }

        [Authorize(Roles = "researchAdministrator")]
        public ActionResult Create()
        {
            return View();
        }
        
        
        [HttpPost]
        [Authorize(Roles = "researchAdministrator")]
        public ActionResult Create(ResearchBlogViewModel model)
        {
            model.Post.Date = DateTime.Now;
            model.Post.UserName = User.Identity.GetUserName();

            rr.AddPost(model.Post);
            if (model.uploadFiles[0] != null) //Den skickar alltid med någon jävel
            {
                foreach (var file in model.uploadFiles) //Kontrollera så att alla filer är under 15MB
                {
                    if (file.ContentLength > 15728640) //15MB i Bytes
                    {
                        ViewBag.Error = file.FileName + " är för stor. Storlek: " + (file.ContentLength / 1024).ToString() + "KB"; ;
                        return View(model);
                    }

                }
                UploadFile(model.uploadFiles, model.Post);

            }
            return RedirectToAction("index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase[] filesToUpload, ResearchPost ownerPost)
        {
            using (DataContext context = new DataContext())
            {
                foreach (var fileToUpload in filesToUpload)
                {
                    var newFile = new UserFile();
                    //newFile.BlogPost = ownerPost;
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
            }
            return RedirectToAction("Index", "Home");

        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            rr.DeletePost(id);
            return RedirectToAction("Index");
        }
    }
    
}