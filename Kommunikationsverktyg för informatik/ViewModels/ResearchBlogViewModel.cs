using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class ResearchBlogViewModel
    {
        public ResearchBlogViewModel()
        {
            this.PostFileCombinations = new List<ResearchPostFileCombo>();
        }
        public ResearchPost Post { get; set; }
        public List<ResearchPost> Posts { get; set; }
        public HttpPostedFileBase[] uploadFiles { get; set; }
        public List<ResearchPostFileCombo> PostFileCombinations { get; set; }
        
    }
    public class ResearchPostFileCombo
    {
        public ResearchPost AttatchedPost { get; set; }
        public List<UserFile> AttatchedDocs { get; set; }
        public List<UserFile> AttatchedPics { get; set; }
        public bool ManyPics { get; set; }

    }
}