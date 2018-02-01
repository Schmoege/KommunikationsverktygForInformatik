using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class MeetingViewModels
    {
        [MaxLength(50)]
        [RegularExpression(@"[A-Öa-ö0-9@#.,!?_-]+", ErrorMessage = "FEL!")]
        [Required, Display(Name = "Plats")]
        public string Place { get; set; }

        [MaxLength(10)]
        [RegularExpression(@"[0-9]{4}[-][0-9]{2}[-][0-9]{2}", ErrorMessage = "FEL!")]
        [Required, Display(Name = "Datum")]
        public string Date { get; set; }

        [MaxLength(5)]
        [RegularExpression(@"[0-9]{2}[:][0-9]{2}", ErrorMessage = "FEL!")]
        [Required, Display(Name = "Tid")]
        public string Time { get; set; }
        public List<string> Times { get; set; }
    }
}