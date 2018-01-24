using DataAccess.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity;
using Kommunikationsverktyg_för_informatik;

namespace DataAccess.Repositories
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            //Hämta exempeldata

            context.SaveChanges();

            base.Seed(context);
        }

        private static void SeedUsers(DataContext context)
        {
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

        }
    }
}
