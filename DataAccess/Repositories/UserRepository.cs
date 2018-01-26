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
            DataContext context = new DataContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            try
            {
                var user = UserManager.FindByName(userName);
                UserManager.AddToRole(user.Id, roleName);
                context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
