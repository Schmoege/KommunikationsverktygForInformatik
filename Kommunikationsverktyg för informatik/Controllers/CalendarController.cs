using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kommunikationsverktyg_för_informatik.ViewModels;
using System.Threading.Tasks;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class CalendarController : Controller
    {
        private MonthViewModels model;
        private int month = DateTime.Today.Month;
        private int year = DateTime.Today.Year;

        // GET: Calendar
        public ActionResult Calendar()
        {
            createModel(PreviousMonth(month));
            return View(model);
        }

        [HttpGet]
        public PartialViewResult nextMonth(int newMonth)
        {
            setMonth(newMonth);
            createModel(PreviousMonth(month));
            return PartialView("_CalendarPartial", model);
        }

        private void setMonth(int newMonth)
        {
            if(newMonth > 12)
            {
                month = 1;
            }
            else
            {
                month = newMonth;
            }
        }

        private void createModel(int oldMonth)
        {
            model = new MonthViewModels
            {
                Name = new DateTime(year, month, DateTime.Today.Day).ToString("MMMM").ToUpper(),
                NumberOfDays = DateTime.DaysInMonth(year, month),
                PreviousMonthsNumberOfDays = DateTime.DaysInMonth(year, oldMonth),
                FirstDayOfMonth = getFirstDayOfMonth(),
                CurrentDay = DateTime.Today.Day,
                CurrentMonth = month
            };
        }

        private int getFirstDayOfMonth()
        {
            int day = Convert.ToInt32(new DateTime(DateTime.Today.Year, month, 1).DayOfWeek);
            if(day == 0)
            {
                day = 7;
            }
            return day;
        }

        //private int nextMonth(int currentMonth)
        //{
        //    int month = currentMonth;
        //    if(currentMonth == 12)
        //    {
        //        month = 1;
        //    }
        //    else
        //    {
        //        month++;
        //    }
        //    return month;
        //}

        private int PreviousMonth(int currentMonth)
        {
            int month = currentMonth;
            if (currentMonth == 1)
            {
                month = 12;
            }
            else
            {
                month--;
            }
            return month;
        }
    }
}