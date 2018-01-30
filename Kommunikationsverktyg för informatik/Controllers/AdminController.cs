using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataAccess.Repositories;
using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.ViewModels;
using static Kommunikationsverktyg_för_informatik.Controllers.ManageController;
using Microsoft.AspNet.Identity.EntityFramework;

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


        [Authorize(Roles = "administrator")]
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

        public ActionResult Edit(string id)
        {

            var ur = new UserRepository();
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id.Equals(id));
                var editViewModel = new EditViewModels(user);
                return View(editViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModels model)
        {
            if (ModelState.IsValid)
            {
                using (DataContext db = new DataContext())
                {
                    var id = Url.RequestContext.RouteData.Values["id"].ToString();
                    var user = db.Users.FirstOrDefault(u => u.Id.Equals(id));
                    UserRepository ur = new UserRepository();

                    if (!user.Id.Equals(User.Identity.GetUserId()))
                    {
                        user.Admin = model.applicationUser.Admin;
                    }

                    if (!user.Admin)
                    {
                        ur.RemoveUserFromRole(user.Email, "administrator");
                        ur.AddUserToRole(user.Email, "user");
                    }
                    else if (user.Admin)
                    {
                        ur.RemoveUserFromRole(user.Email, "user");
                        ur.AddUserToRole(user.Email, "administrator");
                    }

                    user.FirstName = model.applicationUser.FirstName;
                    user.LastName = model.applicationUser.LastName;
                    
                    user.Email = model.applicationUser.Email;
                    user.UserName = model.applicationUser.Email;

                    


                    
                    if (model.setPassword.NewPassword != null)
                    {
                    String userId = user.Id;
                    String newPassword = model.setPassword.NewPassword;
                    String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
                    UserStore<ApplicationUser> store = new UserStore<ApplicationUser>();
                    await store.SetPasswordHashAsync(user, hashedNewPassword);
                    }
                   

                    //if (ModelState.IsValid)
                    //{
                    //    //var resultDeletePass = UserManager.RemovePassword(user.Id);
                    //    //var resultPass = UserManager.AddPassword(User.Identity.GetUserId(), model.setPassword.NewPassword);
                    //    //if (resultPass.Succeeded)
                    //    //{
                    //    //    var userPass = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    //    //    if (userPass != null)
                    //    //    {
                    //    //        await SignInManager.SignInAsync(userPass, isPersistent: false, rememberBrowser: false);
                    //    //    }
                    //    //    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    //    //}
                    //    AddErrors(resultDeletePass);
                    //    AddErrors(resultPass);
                    //}

                    db.SaveChanges();
                    //var result = await UserManager.UpdateAsync(user);
                }
                return RedirectToAction("Adminpanel", "Admin");
            }

            return View(model);

        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}