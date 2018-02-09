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
            this.Location = new Location();
        }

        public Post Post { get; set; }
        public List<Kategori> Kategorier { get; set; }
        public string SelectedCategory { set; get; }
        public Location Location { get; set; }

        public HttpPostedFileBase[] uploadFiles { get; set; }
        public List<PostFileCombo> PostFileCombinations { get; set; }

        public int OldCount { get; set; }
        public int NewCount { get; set; }
        public int DbCount { get; set; }

        public bool Hidden { get; set; } = false;
        public bool Formal { get; set; } = false;
    }
    public class PostFileCombo
    {
        public Post AttatchedPost { get; set; }
        public ApplicationUser AttachedUser { get; set; }
        public List<UserFile> AttatchedDocs { get; set; }
        public List<UserFile> AttatchedPics { get; set; }

    }
}