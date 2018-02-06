using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class GetCommentViewModels
    {
        public ApplicationUser User { get; set; }

        public Comment Comment { get; set; }

        public string ConvertedDateTime { get; set; }
    }
}