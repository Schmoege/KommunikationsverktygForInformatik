﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class DayViewModels
    {
        public string Date { get; set; }
        public List<string> Meetings { get; set; }
        public List<string> Notes { get; set; }
    }
}