﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using DataAccess.Models;

namespace Kommunikationsverktyg_för_informatik.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "First name")]
        [RegularExpression(@"^[a-öA-Ö-]+$", ErrorMessage = "Use letters only in the first name please")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Last name")]
        [RegularExpression(@"^[a-öA-Ö-]+$", ErrorMessage = "Use letters only in the last name please")]
        public string LastName { get; set; }

        public bool Active { get; set; } = true;

        public string BlogDisplayName { get { return string.Format("{0} {1}({2})", FirstName, LastName, Email);  } }

        //public List<Post> UnreadBlogPosts { get; set; }

        //public List<EducationPost> UnreadEducationPosts { get; set; }

        public virtual ICollection<ResearchPost> UnreadResearchPosts { get; set; }
        public virtual ICollection<Post> UnreadPosts { get; set; }
        public virtual ICollection<EducationPost> UnreadEducationPosts { get; set; }

        public ApplicationUser()
        {
            this.UnreadResearchPosts = new HashSet<ResearchPost>();
            this.UnreadEducationPosts = new HashSet<EducationPost>();
            this.UnreadPosts = new HashSet<Post>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}