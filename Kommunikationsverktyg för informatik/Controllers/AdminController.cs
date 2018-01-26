using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataAccess.Repositories;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class AdminController : Controller
    {
      
        private ApplicationUserManager _userManager;
        //
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        
        [Authorize(Roles="administrator")]
        public ActionResult Adminpanel()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Admin = model.Admin };
                var result = await UserManager.CreateAsync(user, model.Password);

                UserRepository ur = new UserRepository();
                if (model.Admin)
                {
                    ur.AddUserToRole(model.Email, "administrator");
                }
                else
                {
                    ur.AddUserToRole(model.Email, "user");
                }
                
                return RedirectToAction("Adminpanel", "Admin");
            }

            return View(model);
        }

        public ActionResult SetActive(ApplicationUser model)
        {
            UserRepository ur = new UserRepository();
            ur.SetActive(model.Email);

            return RedirectToAction("Adminpanel", "Admin");
        }
        public ActionResult SetInactive(ApplicationUser model)
        {
            UserRepository ur = new UserRepository();
            ur.SetInactive(model.Email);

            return RedirectToAction("Adminpanel", "Admin");
        }
    }
}