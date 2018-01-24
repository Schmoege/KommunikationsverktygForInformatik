using DataAccess.Models;
using DataAccess.Repositories;
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
        public ActionResult Index()
        {
            var posts = blogRepository.GetAll();
            return View(posts);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            post.Date = DateTime.Now;
            post.UserName = User.Identity.GetUserName();
            context.Posts.Add(post);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}