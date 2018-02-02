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
        public EducationPost GetPost(Guid id)
        {
            using (DataContext db = new DataContext())
            {
                EducationPost post = db.EducationPosts.SingleOrDefault(p => p.Id.Equals(id));
                return post;
            }
        }
        public void EditPost(EducationPost post)
        {
            using (DataContext db = new DataContext())
            {
                EducationPost postToEdit = GetPost(post.Id);
                postToEdit.Title = post.Title;
                postToEdit.Content = post.Content;
                db.Entry(postToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
