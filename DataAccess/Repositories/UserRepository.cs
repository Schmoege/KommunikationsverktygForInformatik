using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository
    {

        public void AddUserToRole(string userName, string roleName)
        {
            using (var db = new DataContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                try
                {
                    var user = UserManager.FindByName(userName);
                    UserManager.AddToRole(user.Id, roleName);
                    db.SaveChanges();
                }
                catch
                {
                    throw;
                }
            };
        }

        public void RemoveUserFromRole(string userName, string roleName)
        {
            using (var db = new DataContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                try
                {
                    var user = UserManager.FindByName(userName);
                    UserManager.RemoveFromRoles(user.Id, roleName);
                    db.SaveChanges();
                }
                catch
                {
                    throw;
                }
            };
        }

        public ApplicationUser getUser(string email)
        {
            using (var db = new DataContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                try
                {
                    var user = UserManager.FindByName(email);
                    return user;
                }
                catch
                {
                    return null;
                }
            };
        }

        public void SetActive(string email)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(m => m.Email.Equals(email));
                user.Active = true;
                db.SaveChanges();
            };
        }
        public void SetInactive(string email)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(m => m.Email.Equals(email));
                user.Active = false;
                db.SaveChanges();
            };
        }

        public void ChangeFirstname(string email, string firstName)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(m => m.Email.Equals(email));
                user.FirstName = firstName;
                db.SaveChanges();
            };
        }
        public void ChangeLastname(string email, string lastName)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(m => m.Email.Equals(email));
                user.LastName = lastName;
                db.SaveChanges();
            };
        }
        public void ChangeEmail(string email, string newEmail)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(m => m.Email.Equals(email));
                user.Email = newEmail;
                db.SaveChanges();
            };
        }
        public void ChangeAdmin(string email, bool admin)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(m => m.Email.Equals(email));
                if (user.Admin && !admin)
                {
                    RemoveUserFromRole(user.Email, "administrator");
                    AddUserToRole(user.Email, "user");
                }
                else if (!user.Admin && admin)
                {
                    RemoveUserFromRole(user.Email, "user");
                    AddUserToRole(user.Email, "administrator");
                }
                user.Admin = admin;
                db.SaveChanges();
            };
        }


    }

}
