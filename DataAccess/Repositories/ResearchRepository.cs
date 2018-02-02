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
                List <ResearchPost> listOfPosts = new List<ResearchPost>();
                listOfPosts.AddRange(db.ResearchPosts.ToList().OrderByDescending(x => x.Date).Take(10));
                foreach (var post in listOfPosts)
                {
                    post.ApplicationUser = db.Users.SingleOrDefault(x => x.Id.Equals(post.UserId));
                }
                return listOfPosts;
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

        public void DeletePost(Guid id)
        {
            using (DataContext context = new DataContext())
            {
                ResearchPost postToDelete = context.ResearchPosts.Find(id);
                context.ResearchPosts.Remove(postToDelete);
                context.SaveChanges();

            }

        }
        public ResearchPost GetPost(Guid id)
        {
            using (DataContext db = new DataContext())
            {
                ResearchPost post = db.ResearchPosts.SingleOrDefault(p => p.Id.Equals(id));
                var user = db.Users.SingleOrDefault(x => x.Id.Equals(post.UserId));
                post.ApplicationUser = user;
                return post;
            }
        }
        public void EditPost(ResearchPost post)
        {
            using (DataContext db = new DataContext())
            {
                ResearchPost postToEdit = GetPost(post.Id);
                postToEdit.Title = post.Title;
                postToEdit.Content = post.Content;
                db.Entry(postToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
