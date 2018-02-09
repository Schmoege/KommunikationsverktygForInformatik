using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class TimeSuggestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TID { get; set; }

        [Required]
        public string Suggestion { get; set; }
        
        [DefaultValue(false)]
        public bool ConfirmedTime { get; set; }

        [ForeignKey("Meeting")]
        public int MeetingID { get; set; }
        public Meeting Meeting { get; set; }
    }
}
