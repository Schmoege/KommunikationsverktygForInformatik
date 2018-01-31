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
    public class Invitation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date { get; set; }

        [DefaultValue(0)]
        public int Answer { get; set; }

        [DefaultValue(false)]
        public bool Answered { get; set; }

        [ForeignKey("Meeting")]
        public int MeetingID { get; set; }
        public Meeting Meeting { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
