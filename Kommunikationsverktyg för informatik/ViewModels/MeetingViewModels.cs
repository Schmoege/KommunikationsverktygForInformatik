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
        [MaxLength(100)]
        [RegularExpression(@"[A-Öa-ö0-9@#., !?_-]+", ErrorMessage = "Inga specialtecken.")]
        [Required, Display(Name = "Ämne")]
        public string Subject { get; set; }

        [MaxLength(50)]
        [RegularExpression(@"[A-Öa-ö0-9@#., !?_-]+", ErrorMessage = "Inga specialtecken.")]
        [Required, Display(Name = "Plats")]
        public string Place { get; set; }

        [MaxLength(10)]
        [RegularExpression(@"[0-9]{4}[-][0-9]{2}[-][0-9]{2}", ErrorMessage = "Fel format, skall följa YYYY-MM-DD.")]
        [System.Web.Mvc.Remote(action: "CheckDate",controller: "Meeting", HttpMethod = "POST", ErrorMessage = "Datumet får icke vara tidigare än idag och måste vara ett korrekt datum.")]
        [Required, Display(Name = "Datum")]
        public string Date { get; set; }

        [MaxLength(5)]
        [RegularExpression(@"[0-9]{2}[:][0-9]{2}", ErrorMessage = "Fel format, skall följa hh:mm.")]
        [Required, Display(Name = "Tid")]
        public string Time { get; set; }
        public List<string> Times { get; set; }
    }
}