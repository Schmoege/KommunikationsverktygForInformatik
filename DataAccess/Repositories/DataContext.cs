using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DataAccess.Models
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
