using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kommunikationsverktyg_för_informatik.ViewModels;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Calendar()
        {
            var model = new MonthViewModels
            {
                Name = DateTime.Today.ToString("MMMM").ToUpper(),
                NumberOfDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month),
                PreviousMonthsNumberOfDays = DateTime.DaysInMonth(DateTime.Now.Year, PreviousMonth(DateTime.Now.Month)),
                FirstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).Day,
                CurrentDay = DateTime.Today.Day
            };
            return View(model);
        }

        private int PreviousMonth(int currentMonth)
        {
            int month = 0;
            if (currentMonth == 1)
            {
                month = 12;
            }
            else
            {
                month = currentMonth - 1;
            }
            return month;
        }
    }
}