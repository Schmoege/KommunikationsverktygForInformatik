﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class MonthViewModels
    {
        public string Name { get; set; }
        public int NumberOfDays { get; set; }
        public int PreviousMonthsNumberOfDays { get; set; }
        public int FirstDayOfMonth { get; set; }
        public int CurrentDay { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
        public List<Meeting> Meetings { get; set; }
    }
}