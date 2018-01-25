using DataAccess.Repositories;
using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DataAccess.Models
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DataInitializer());
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        
    }
}
