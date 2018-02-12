using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class FrontPageViewModel
    {
        public List<ResearchPost> ResearchPosts { get; set; }
        public List<ApplicationUser> ResearchUsers { get; set; }

        public List<EducationPost> EducationPosts { get; set; }
        public List<ApplicationUser> EducationUsers { get; set; }

        public List<Post> Posts { get; set; }
        public List<Post> FormalPosts { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public FrontPageViewModel()
        {
            Users = new List<ApplicationUser>();
            ResearchUsers = new List<ApplicationUser>();
            EducationUsers = new List<ApplicationUser>();
        }

    }
}