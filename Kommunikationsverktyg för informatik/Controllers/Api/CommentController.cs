using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.Models;
using Kommunikationsverktyg_för_informatik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kommunikationsverktyg_för_informatik.Controllers.Api
{
    public class CommentController : ApiController
    {
        DataContext db = new DataContext();

        [HttpPost]
        public List<GetCommentViewModels> GetComments(Post post)
        {
            List<Comment> comments =  db.Comments.
                Where(x => x.PostID.ToString() == post.Id).OrderByDescending(x => x.PostedAt).ToList();
            var userIDs = comments.Select(x => x.Author);
            List<ApplicationUser> users = db.Users
                .Where(x => userIDs.Contains(x.Id)).ToList();

            List <GetCommentViewModels> model = new List<GetCommentViewModels>();
            foreach (var item in comments)
            {
                var commentuser = users.SingleOrDefault(x => x.Id == item.Author);
                var input = new GetCommentViewModels();
                input.User = commentuser;
                input.Comment = item;
                input.ConvertedDateTime = item.PostedAt.ToString();
                model.Add(input);
            }
            return model;
        }

        [Route("api/comment/postcomment")]
        [HttpPost]
        public PostCommentViewModels PostComment(PostCommentViewModels model)
        {
            if(model.Content.Length < 1)
            {
                return model;
            }
            Comment newComment = new Comment
            {
                Author = model.Author,
                Content = model.Content,
                PostID = model.PostID,
                PostedAt = DateTime.Now,
            };
            db.Comments.Add(newComment);
            db.SaveChanges();

            model.Author = newComment.Author;
            model.Content = newComment.Content;
            model.PostID = newComment.PostID;
            model.ConvertedDateTime = newComment.PostedAt.ToString();
            return model;
        }
    }
}
