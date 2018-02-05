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
    }
}