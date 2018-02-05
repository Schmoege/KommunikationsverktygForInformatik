using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class MeetingController : Controller
    {
        // GET: Meeting
        public ActionResult Meeting()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult CreateMeeting()
        {
            return PartialView("_CreateMeetingPartial");
        }

        [HttpGet]
        public PartialViewResult ViewMeetings()
        {
            return PartialView("_MeetingPartial");
        }
        
        [HttpPost]
        public ActionResult CheckDate([Bind(Prefix = "Item1.Date")] string dateToControl)
        {
            bool validDate = false;
            try
            {
                validDate = IsValidDate(dateToControl) ? true : false;
                return Json(validDate);
            }
            catch
            {
                return Json(false);
            }
        }

        private bool IsValidDate(string dateToControl)
        {
            DateTime date = new DateTime(Convert.ToInt32(dateToControl.Substring(0, 4))
                                         , Convert.ToInt32(dateToControl.Substring(5, 2))
                                         , Convert.ToInt32(dateToControl.Substring(8, 2)));
            DateTime today = DateTime.Today;
            if(DateTime.Compare(date, today) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}