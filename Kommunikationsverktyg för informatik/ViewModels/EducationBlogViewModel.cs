using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class EducationBlogViewModel
    {
        public EducationBlogViewModel()
        {
            this.PostFileCombinations = new List<EducationPostFileCombo>();
        }
        public EducationPost Post { get; set; }
        public List<EducationPost> Posts { get; set; }
        public HttpPostedFileBase[] uploadFiles { get; set; }
        public List<EducationPostFileCombo> PostFileCombinations { get; set; }

    }
    public class EducationPostFileCombo
    {
        public EducationPost AttatchedPost { get; set; }
        public List<UserFile> AttatchedDocs { get; set; }
        public List<UserFile> AttatchedPics { get; set; }
        public bool ManyPics { get; set; }

    }
}