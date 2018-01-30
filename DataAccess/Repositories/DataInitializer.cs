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

            context.SaveChanges();
            base.Seed(context);

            


            UserRepository ur = new UserRepository();
            ur.AddUserToRole("admin@admin.se", "administrator");


            

        }
    }
}
