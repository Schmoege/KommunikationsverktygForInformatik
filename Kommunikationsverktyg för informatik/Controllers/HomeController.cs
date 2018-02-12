using DataAccess.Models;
using DataAccess.Repositories;
using Kommunikationsverktyg_för_informatik.Models;
using Kommunikationsverktyg_för_informatik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kommunikationsverktyg_för_informatik.Controllers
{

    public class HomeController : Controller
    {
        DataContext context = new DataContext();

        private BlogRepository blogRepository;
        public HomeController()
        {
            DataContext dataContext = new DataContext();
            blogRepository = new BlogRepository(dataContext);
        }
        [Authorize]
        public ActionResult Index(FrontPageViewModel model)
        {
            if(model == null)
            {
                model = new FrontPageViewModel();
            }
            var informalCategories = context.Categories.Where(c => !c.Formell);
           
            model.Posts = context.Posts.Where(x => x.Hidden == false && informalCategories.Select(c => c.Id).Contains(x.KategoriId)).Take(1).OrderByDescending(x => x.Date).ToList();
            foreach (var item in model.Posts)
            {
                model.Users.Add(context.Users.SingleOrDefault(x => x.Id == item.User));
            }
            model.EducationPosts = context.EducationPosts.Take(1).OrderByDescending(x => x.Date).ToList();
            foreach (var item in model.EducationPosts)
            {
                model.EducationUsers.Add(context.Users.SingleOrDefault(x => x.Id == item.UserId));
            }
            model.ResearchPosts = context.ResearchPosts.Take(1).OrderByDescending(x => x.Date).ToList();
            foreach (var item in model.ResearchPosts)
            {
                model.ResearchUsers.Add(context.Users.SingleOrDefault(x => x.Id == item.UserId));
            }
            model.FormalPosts = context.Posts.Where(x => x.Hidden == false && !informalCategories.Select(c => c.Id).Contains(x.KategoriId)).Take(1).OrderByDescending(x => x.Date).ToList();
            return View(model);

        }
    


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}