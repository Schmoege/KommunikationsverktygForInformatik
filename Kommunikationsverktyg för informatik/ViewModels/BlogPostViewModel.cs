using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class BlogPostViewModel
    {
        public Post Post { get; set; }
        public List<Kategori> Kategorier { get; set; }
        public List<Post> Posts { get; set; }
        public string SelectCategories { set; get; }
        public HttpPostedFileBase uploadFile { get; set; }
    }
}