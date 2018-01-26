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


    }
}