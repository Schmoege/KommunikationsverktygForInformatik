﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BlogRepository
    {
        public DataContext dataContext;

        public BlogRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public BlogRepository()
        {

        }


        public IEnumerable<Post> GetAll(bool formal)
        {
            
            using (var db = new DataContext())
            {
                var categories = db.Categories
                 .Where(x => x.Formell == formal).Select(x => x.Id);
                List<Post> postList = db.Posts.Where(x => x.Hidden == false && categories.Contains(x.KategoriId)).OrderByDescending(x => x.Date).Take(5).ToList();
                foreach (var p in postList)
                {
                    p.Location = db.Location.SingleOrDefault(l => l.PostId.ToString().Equals(p.Id.ToString()));
                }
                return postList;
            }


            
            /*return dataContext.Posts.Where(x => x.Hidden == false && categories.Contains(x.KategoriId)).OrderByDescending(x => x.Date).Take(5);*/
        }

        public void AddPost(Post post)
        {
            using (DataContext db = new DataContext())
            {
                var users = db.Users.ToList();
                users.ForEach(u => u.UnreadPosts.Add(post));
                db.Posts.Add(post);
                db.SaveChanges();
            }
        }

        public int GetUnreadPosts(string userId)
        {
            using (DataContext db = new DataContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Id.Equals(userId));

                int unread = user.UnreadPosts.Count();
                return unread;
            }
        }
        public void ClearUnreadPosts(string userId)
        {
            using (DataContext db = new DataContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Id.Equals(userId));
                user.UnreadPosts.Clear();
                db.SaveChanges();
            }
        }

        public Location GetLocation(string postId)
        {
            using (var db = new DataContext())
            {
                try
                {
                    Location loc = db.Location.Find(postId);
                    return loc;
                }
                catch
                {
                    return new Location();
                }
            }
        }

    }
}