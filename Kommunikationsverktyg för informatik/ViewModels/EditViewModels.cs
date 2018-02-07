using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class EditViewModels
    {
        public ApplicationUser applicationUser { get; set; }
        public SetPasswordViewModel setPassword { get; set; }
        public string Id;

        //[System.Web.Mvc.Remote("IsEmailAvailable", "Validation")]
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

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

        [Required]
        [Display(Name = "Admin")]
        public bool Admin { get; set; }

        [Required]
        [Display(Name = "Education Admin")]
        public bool EducationAdmin { get; set; }

        [Required]
        [Display(Name = "Research Admin")]
        public bool ResarchAdmin { get; set; }

        public EditViewModels(ApplicationUser user)
        {
            UserRepository ur = new UserRepository();
            this.applicationUser = user;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            if (ur.IsUserInRole(user.Id, "administrator"))
            {
                this.Admin = true;
            }
            if (ur.IsUserInRole(user.Id, "researchAdministrator"))
            {
                this.ResarchAdmin = true;
            }
            if (ur.IsUserInRole(user.Id, "educationAdministrator"))
            {
                this.EducationAdmin = true;
            }
            //this.registerViewModel.FirstName = user.FirstName;
            //this.registerViewModel.LastName = user.FirstName;
            //this.registerViewModel.Email = user.Email;
        }

        public EditViewModels()
        {

        }
    }
    
}