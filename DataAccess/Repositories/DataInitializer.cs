using DataAccess.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity;
using Kommunikationsverktyg_för_informatik;
using System;

namespace DataAccess.Repositories
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {

            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("Abc123");
            var admin = new ApplicationUser()
            {
                UserName = "admin@admin.se",
                Email = "admin@admin.se",
                FirstName = "Admin",
                LastName = "Adminsson",
                Admin = true,
                PasswordHash = password,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var user1 = new ApplicationUser()
            {
                UserName = "Johan@user.se",
                Email = "Johan@user.se",
                FirstName = "Johan",
                LastName = "Johansson",
                Admin = false,
                PasswordHash = passwordHash.HashPassword("Abc123"),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var user2 = new ApplicationUser()
            {
                UserName = "Lisa@user.se",
                Email = "Lisa@user.se",
                FirstName = "Lisa",
                LastName = "Einarsdottír",
                Admin = false,
                PasswordHash = passwordHash.HashPassword("Abc123"),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var adminrole = new IdentityRole()
            {
                Name = "administrator"
            };
            var userrole = new IdentityRole()
            {
                Name = "user"
            };

            context.Users.Add(admin);
            context.Roles.Add(adminrole);
            context.Roles.Add(userrole);
            context.Users.Add(user1);
            context.Users.Add(user2);

            context.SaveChanges();
            base.Seed(context);
            
            UserRepository ur = new UserRepository();
            ur.AddUserToRole("admin@admin.se", "administrator");
            ur.AddUserToRole("Lisa@user.se", "user");
            ur.AddUserToRole("Johan@user.se", "user");
        }
    }
}
