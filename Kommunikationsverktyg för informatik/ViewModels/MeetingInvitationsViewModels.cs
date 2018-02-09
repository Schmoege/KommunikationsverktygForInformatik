using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class MeetingInvitationsViewModels
    {
        public string Sender { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }

        public string Place { get; set; }
        
        public bool Can { get; set; }

        public bool Answered { get; set; }

        public int MeetingID { get; set; }

        public List<string> SuggestionsOfTimes { get; set; }
    }
}