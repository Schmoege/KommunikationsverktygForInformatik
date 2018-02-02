using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ResearchRepository
    {


        public IEnumerable<ResearchPost> GetAll()
        {
            using (DataContext db = new DataContext())
            {
                return db.ResearchPosts.ToList().OrderByDescending(x => x.Date).Take(10);
            }

        }

        public void AddPost(ResearchPost post)
        {
            using (DataContext db = new DataContext())
            {
                db.ResearchPosts.Add(post);
                db.SaveChanges();
            }
        }

        public void DeletePost(int id)
        {
            using (DataContext context = new DataContext())
            {
                ResearchPost postToDelete = context.ResearchPosts.Find(id);
                context.ResearchPosts.Remove(postToDelete);
                context.SaveChanges();

            }

        }
    }
}
