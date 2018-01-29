using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kommunikationsverktyg_för_informatik.Models;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class BlogPostViewModel
    {
        public BlogPostViewModel()
        {
            this.PostFileCombinations = new List<PostFileCombo>();
        }

        public Post Post { get; set; }
        public List<Kategori> Kategorier { get; set; }
        public List<Post> Posts { get; set; }
        public string SelectCategories { set; get; }
        
        public HttpPostedFileBase uploadFile { get; set; }
        public List<PostFileCombo> PostFileCombinations { get; set; }
        public ApplicationUser currentUser { get; set; }

    }
    public class PostFileCombo
    {
        public Post AttatchedPost { get; set; }
        public UserFile AttatchedFile { get; set; }
        
    }
}