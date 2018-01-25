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
                Name = new DateTime(DateTime.Today.Year, month, DateTime.Today.Day).ToString("MMMM").ToUpper(),
                NumberOfDays = DateTime.DaysInMonth(DateTime.Now.Year, month),
                PreviousMonthsNumberOfDays = DateTime.DaysInMonth(DateTime.Now.Year, oldMonth),
                FirstDayOfMonth = new DateTime(DateTime.Today.Year, month, 1).Day,
                CurrentDay = DateTime.Today.Day,
                CurrentMonth = month
            };
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