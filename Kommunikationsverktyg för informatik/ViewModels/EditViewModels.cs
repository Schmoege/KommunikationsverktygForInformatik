using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class EditViewModels
    {
        public ApplicationUser applicationUser { get; set; }
        public SetPasswordViewModel setPassword { get; set; }
        public string Id;

        public EditViewModels(ApplicationUser user)
        {
            this.applicationUser = user;
        }
        public EditViewModels()
        {

        }
    }
    
}