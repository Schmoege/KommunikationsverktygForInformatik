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
        private DayViewModels dayModel;
        private int month = DateTime.Today.Month;
        private int year = DateTime.Today.Year;

        // GET: Calendar
        public ActionResult Calendar()
        {
            createModel(previousMonth(month));
            return View(model);
        }

        [HttpGet]
        public PartialViewResult nextMonth(int newMonth, int newYear)
        {
            setMonth(newMonth);
            setYear(newYear);
            createModel(previousMonth(month));
            return PartialView("_CalendarPartial", model);
        }

        [HttpGet]
        public PartialViewResult previousMonth(int newMonth, int newYear)
        {
            setMonth(newMonth);
            setYear(newYear);
            createModel(previousMonth(month));
            return PartialView("_CalendarPartial", model);
        }

        [HttpGet]
        public PartialViewResult Month(int month, int year)
        {
            setMonth(month);
            setYear(year);
            createModel(previousMonth(month));
            return PartialView("_CalendarPartial", model);
        }

        [HttpGet]
        public PartialViewResult day(string year, string month, string day)
        {
            dayModel = new DayViewModels
            {
                Year = Convert.ToInt32(year),
                Month = Convert.ToInt32(month),
                Day = Convert.ToInt32(day),
                Meetings = null,
                Notes = null
            };
            return PartialView("_CalendarDayPartial", dayModel);
        }

        private void setMonth(int newMonth)
        {
            if(newMonth > 12)
            {
                month = 1;
            }
            else if(newMonth <= 0)
            {
                month = 12;
            }
            else
            {
                month = newMonth;
            }
        }

        private void setYear(int newYear)
        {
            year = newYear;
        }

        private void createModel(int oldMonth)
        {
            model = new MonthViewModels
            {
                Name = new DateTime(year, month, 1).ToString("MMMM").ToUpper(),
                NumberOfDays = DateTime.DaysInMonth(year, month),
                PreviousMonthsNumberOfDays = DateTime.DaysInMonth(year, oldMonth),
                FirstDayOfMonth = getFirstDayOfMonth(),
                CurrentDay = DateTime.Today.Day,
                CurrentMonth = month,
                CurrentYear = year
            };
        }

        private int getFirstDayOfMonth()
        {
            int day = Convert.ToInt32(new DateTime(year, month, 1).DayOfWeek);
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

        private int previousMonth(int currentMonth)
        {
            int month = currentMonth;
            if (currentMonth == 1)
            {
                month = 12;
            }
            else
            {
                month = month - 1;
            }
            return month;
        }
    }
}