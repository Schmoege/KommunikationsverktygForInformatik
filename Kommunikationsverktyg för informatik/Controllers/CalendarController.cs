using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kommunikationsverktyg_för_informatik.ViewModels;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    [Authorize]
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
            DayMeetings(year, month, day);
            return PartialView("_CalendarDayPartial", dayModel);
        }

        private void DayMeetings(string year, string month, string day)
        {
            MeetingRepository mr = new MeetingRepository();
            string date = "";
            if(Convert.ToInt32(month) < 10)
            {
                date = year + "-0" + month + "-";
            }
            else
            {
                date = year + "-" + month + "-";
            }
            if(Convert.ToInt32(day) < 10)
            {
                date += "0" + day;
            }
            else
            {
                date += day;
            }
            List<Meeting> meetings = new List<Meeting>();
            meetings = mr.GetAllConfirmedMeetingsDay(date);
            dayModel = new DayViewModels
            {
                Year = Convert.ToInt32(year),
                Month = Convert.ToInt32(month),
                Day = Convert.ToInt32(day),
                Meetings = meetings,
                Notes = null
            };
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
            MeetingRepository mr = new MeetingRepository();
            List<Meeting> meetings = new List<Meeting>();
            meetings = mr.GetAllConfirmedMeetings();
            model = new MonthViewModels
            {
                Name = new DateTime(year, month, 1).ToString("MMMM").ToUpper(),
                NumberOfDays = DateTime.DaysInMonth(year, month),
                PreviousMonthsNumberOfDays = DateTime.DaysInMonth(year, oldMonth),
                FirstDayOfMonth = getFirstDayOfMonth(),
                CurrentDay = DateTime.Today.Day,
                CurrentMonth = month,
                CurrentYear = year,
                Meetings = meetings
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