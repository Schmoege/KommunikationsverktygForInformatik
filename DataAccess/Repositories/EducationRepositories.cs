using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class EducationRepository
    {


        public IEnumerable<EducationPost> GetAll()
        {
            using (DataContext db = new DataContext())
            {
                return db.EducationPosts.ToList().OrderByDescending(x => x.Date).Take(10);
            }

        }

        public void AddPost(EducationPost post)
        {
            using (DataContext db = new DataContext())
            {
                db.EducationPosts.Add(post);
                db.SaveChanges();
            }
        }

        public void DeletePost(Guid id)
        {
            using (DataContext context = new DataContext())
            {
                EducationPost postToDelete = context.EducationPosts.Find(id);
                context.EducationPosts.Remove(postToDelete);
                context.SaveChanges();

            }

        }
    }
}
