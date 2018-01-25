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
            string password = passwordHash.HashPassword("Hej123!");
            var admin = new ApplicationUser()   
            {
                UserName = "Admin@admin.se",
                Email = "Admin@admin.se",
                PasswordHash = password,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            context.Users.Add(admin);

            base.Seed(context);
          
        }
    }
}
