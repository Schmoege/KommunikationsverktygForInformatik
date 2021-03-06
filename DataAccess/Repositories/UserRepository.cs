﻿using DataAccess.Models;
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

        public ApplicationUser GetUser(string id)
        {
            using (var db = new DataContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                try
                {
                    var user = UserManager.FindById(id);
                    return user;
                }
                catch
                {
                    return null;
                }
            };
        }
        public ApplicationUser GetUserByEmail(string email)
        {
            using (var db = new DataContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                try
                {
                    var user = UserManager.FindByEmail(email);
                    return user;
                }
                catch
                {
                    return null;
                }
            };
        }

        public List<ApplicationUser> GetUserList()
        {
            using (var db = new DataContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                try
                {
                    List<ApplicationUser> userList = UserManager.Users.ToList<ApplicationUser>();
                    return userList;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string GetUsername(string id)
        {
            using (DataContext db = new DataContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Id.Equals(id));
                return user.UserName;
            }
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
                if (!admin)
                {
                    RemoveUserFromRole(user.Email, "administrator");
                    AddUserToRole(user.Email, "user");
                }
                else if (admin)
                {
                    RemoveUserFromRole(user.Email, "user");
                    AddUserToRole(user.Email, "administrator");
                }
               
                db.SaveChanges();
            };
        }

        public int GetRolesAssignedByRoleName(string roleName)
        {

            using (var db = new DataContext())
            {
                
                var role = db.Set<IdentityRole>().FirstOrDefault(r => r.Name.Equals(roleName));

                int number = db.Set<IdentityUserRole>().Count(r => r.RoleId == role.Id);
                return number;
            }
        }

        public bool IsUserInRole(string id, string roleName)
        {
            using (var db = new DataContext())
            {

                var role = db.Set<IdentityRole>().FirstOrDefault(r => r.Name.Equals(roleName)).Id;
                

                var exists = db.Set<IdentityUserRole>().FirstOrDefault(r => r.UserId.Equals(id) && r.RoleId.Equals(role));
                if (exists != null) return true;
                else return false;
            }
        }

        public void EditUser(ApplicationUser user, string userId)
        {

            using (DataContext db = new DataContext())
            {
                var userToEdit = db.Users.SingleOrDefault(u => u.Id.Equals(userId));
                userToEdit.FirstName = user.FirstName;
                userToEdit.LastName = user.LastName;
                userToEdit.Email = user.Email;
                userToEdit.UserName = user.Email;
                db.Entry(userToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

        }

        public bool CheckEmailAvailable(string email)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    var user = db.Users.Single(u => u.Email.Equals(email));
                    return false;
                }
                catch
                {
                    return true;
                }
            }
        }
    }

}
