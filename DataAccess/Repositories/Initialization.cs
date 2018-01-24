using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class Initialization
    {
        public static void Initialize()
        {
            Database.SetInitializer(new DataInitializer()); 

            using (var db = new DataContext())
            {
                {
                    db.Database.Initialize(true);
                }
            }
        }
    }
}
