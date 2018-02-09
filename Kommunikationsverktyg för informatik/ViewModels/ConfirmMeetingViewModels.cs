using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class ConfirmMeetingViewModels
    {
        public string Subject { get; set; }

        public string Place { get; set; }

        public string Date { get; set; }

        public int meetingID { get; set; }

        public Dictionary<string, int> Times { get; set; }
    }
}