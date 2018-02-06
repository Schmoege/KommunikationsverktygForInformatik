using Kommunikationsverktyg_för_informatik.ViewModels;
using DataAccess.Repositories;
using DataAccess.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class MeetingController : Controller
    {
        // GET: Meeting
        public ActionResult Meeting()
        {
            var userID = User.Identity.GetUserId();
            List<MeetingInvitationsViewModels> meetings = FetchInvitedMeetings(userID);
            return View(meetings);
        }

        [HttpGet]
        public PartialViewResult CreateMeeting()
        {
            UserRepository ur = new UserRepository();
            var list = ur.GetUserList();
            List<string> names = new List<string>();
            List<string> mails = new List<string>();
            foreach(var v in list)
            {
                names.Add(v.LastName + ", " + v.FirstName);
                mails.Add(v.Email);
            }
            var model = new MeetingViewModels
            {
                Names = names,
                Mails = mails
            };
            
            return PartialView("_CreateMeetingPartial", Tuple.Create(model, new DateTime()));
        }

        [HttpGet]
        public PartialViewResult ViewMeetings()
        {
            var userID = User.Identity.GetUserId();
            List<MeetingInvitationsViewModels> meetings = FetchInvitedMeetings(userID);
            return PartialView("_MeetingPartial", Tuple.Create(meetings));
        }

        private static List<MeetingInvitationsViewModels> FetchInvitedMeetings(string userID)
        {
            MeetingRepository mr = new MeetingRepository();
            var list = mr.GetInvitedMeetings(userID);
            List<MeetingInvitationsViewModels> meetings = new List<MeetingInvitationsViewModels>();
            foreach (Meeting meeting in list)
            {
                var user = mr.GetMeetingCreator(meeting.MID);
                var mod = new MeetingInvitationsViewModels
                {
                    Subject = meeting.Subject,
                    Place = meeting.Place,
                    Date = meeting.Date,
                    Sender = user.FirstName + user.LastName
                };
                meetings.Add(mod);
            }

            return meetings;
        }

        [HttpPost]
        public ActionResult CreatedMeetings(string subject, string place, string date, string creatorMail, List<string> times, List<string> mails)
        {
            UserRepository ur = new UserRepository();
            MeetingRepository mr = new MeetingRepository();
            var meetingModel = new Meeting
            {
                Subject = subject,
                Place = place,
                Date = date
            };
            var creator = ur.GetUserByEmail(creatorMail);
            var invitationModel = new Invitation
            {
                //Meeting = meetingModel,
                Date = DateTime.Now,
                MeetingID = meetingModel.MID,
                //ApplicationUser = creator,
                UserID = creator.Id
            };
            List<RecieveMeetingInvitation> RMInviteList = new List<RecieveMeetingInvitation>();
            foreach(string mail in mails)
            {
                var RMInvite = new RecieveMeetingInvitation
                {
                    InvitationID = invitationModel.IID,
                    UserID = ur.GetUserByEmail(mail).Id
                };
                RMInviteList.Add(RMInvite);
            }
            List<TimeSuggestion> timeList = new List<TimeSuggestion>();
            List<TimeAnswer> timeAnswerList = new List<TimeAnswer>();
            foreach (string time in times)
            {
                var timeSuggestionModel = new TimeSuggestion
                {
                    MeetingID = meetingModel.MID,
                    Suggestion = time
                };
                foreach(string mail in mails)
                {
                    var user = ur.GetUserByEmail(mail);
                    var timeAnswerModel = new TimeAnswer
                    {
                        UserID = user.Id,
                        TimeID = timeSuggestionModel.TID
                    };
                    timeAnswerList.Add(timeAnswerModel);
                }
                timeList.Add(timeSuggestionModel);
            }
            mr.AddMeeting(meetingModel, invitationModel, RMInviteList, timeList, timeAnswerList);
            return RedirectToAction("ViewMeetings");
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