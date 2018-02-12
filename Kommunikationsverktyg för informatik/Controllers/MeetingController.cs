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
using System.Web.Helpers;
using Kommunikationsverktyg_för_informatik.Models;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    [Authorize]
    public class MeetingController : Controller
    {
        // GET: Meeting
        public ActionResult Meeting()
        {
            var userID = User.Identity.GetUserId();
            List<MeetingInvitationsViewModels> meetings = FetchInvitedMeetings(userID);
            List<MeetingInvitationsViewModels> createdMeetings = FetchCreatedMeetings(userID);
            return View(Tuple.Create(meetings, createdMeetings));
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
            List<MeetingInvitationsViewModels> createdMeetings = FetchCreatedMeetings(userID);
            return PartialView("_MeetingPartial", Tuple.Create(meetings, createdMeetings));
        }

        private static List<MeetingInvitationsViewModels> FetchCreatedMeetings(string userID)
        {
            MeetingRepository mr = new MeetingRepository();
            var list = mr.GetCreatedMeetings(userID);
            List<MeetingInvitationsViewModels> meetings = new List<MeetingInvitationsViewModels>();
            foreach (Meeting meeting in list)
            {
                var user = mr.GetMeetingCreator(meeting.MID);
                var answered = mr.GetAnswer(meeting.MID, userID);
                var mod = new MeetingInvitationsViewModels
                {
                    Subject = meeting.Subject,
                    Place = meeting.Place,
                    Date = meeting.Date,
                    MeetingID = meeting.MID,
                    Sender = user.FirstName + user.LastName,
                    Answered = answered,
                    Confirmed = meeting.Confirmed
                };
                meetings.Add(mod);
            }

            return meetings;
        }

        private static List<MeetingInvitationsViewModels> FetchInvitedMeetings(string userID)
        {
            MeetingRepository mr = new MeetingRepository();
            var list = mr.GetInvitedMeetings(userID);
            List<MeetingInvitationsViewModels> meetings = new List<MeetingInvitationsViewModels>();
            foreach (Meeting meeting in list)
            {
                var user = mr.GetMeetingCreator(meeting.MID);
                var answered = mr.GetAnswer(meeting.MID, userID);
                var mod = new MeetingInvitationsViewModels
                {
                    Subject = meeting.Subject,
                    Place = meeting.Place,
                    Date = meeting.Date,
                    MeetingID = meeting.MID,
                    Sender = user.FirstName + user.LastName,
                    Answered = answered,
                    Confirmed = meeting.Confirmed
                };
                meetings.Add(mod);
            }

            return meetings;
        }

        [HttpPost]
        public ActionResult CreatedMeetings(MeetingViewModels obj, string subject,string place, string date, string creatorMail, string reicevMail, List<string> times, List<string> mails)
        {

            //Configuring webMail class to send emails  
            //gmail smtp server  
            WebMail.SmtpServer = "smtp.gmail.com";
            //gmail port to send emails  
            WebMail.SmtpPort = 587;
            WebMail.SmtpUseDefaultCredentials = true;
            //sending emails with secure protocol  
            WebMail.EnableSsl = true;
            //EmailId used to send emails from application  
            WebMail.UserName = "bimbosson@gmail.com";
            WebMail.Password = "bimbobimbo11";
           


            DataContext db = new DataContext();
            UserRepository ur = new UserRepository();
            MeetingRepository mr = new MeetingRepository();
            var meetingModel = new Meeting
            {
                Subject = subject,
                Place = place,
                Date = date,
               
        
            };
            var creator = ur.GetUserByEmail(creatorMail);

            var invitationModel = new Invitation
            {
                //Meeting = meetingModel,
                Date = DateTime.Now,
                MeetingID = meetingModel.MID,
                //ApplicationUser = creator,
                UserID = creator.Id,
            };

            var f = db.Users.Single(x => x.Id == invitationModel.UserID);
            WebMail.From = f.Email;
            List<RecieveMeetingInvitation> RMInviteList = new List<RecieveMeetingInvitation>();
            RecieveMeetingInvitation receiver = new RecieveMeetingInvitation();
            string allTimes = "";
            foreach (string time in times)
            {
                allTimes += time + "<br>";
            }
            foreach (string mail in mails)
            {
                var RMInvite = new RecieveMeetingInvitation
                {
                    InvitationID = invitationModel.IID,
                    UserID = ur.GetUserByEmail(mail).Id
                };
                RMInviteList.Add(RMInvite);
                var t = db.Users.Single(x => x.Id == RMInvite.UserID);
                string body = String.Format("Du är inbjuden till mötet på/i {0} den {1} med dessa tidsförslag : <br> {2} <br> Vänligen logga in på <a href = 'https://github.com/'>hemsidan</a> för att svara på inbjudan. <hr> Det här är en automatisk inbjudan från Informatiks webbtjänst. <br> Svara inte på detta mejl.", 
                    place, date, allTimes);
                WebMail.Send(from: f.Email, to: t.Email, subject: obj.Subject, body:body, isBodyHtml: true);
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
        public ActionResult AnswerMeeting(int meetingID, int answer, List<string> times)
        {
            MeetingRepository rm = new MeetingRepository();
            string userID = User.Identity.GetUserId();
            rm.SetMeetingAnswer(answer, meetingID, userID);
            if (answer == 1)
            {
                foreach(string time in times)
                {
                    rm.SetTimeAnswer(meetingID, time, userID);
                }
            }
            return RedirectToAction("ViewMeetings");
        }

        [HttpGet]
        public PartialViewResult SpecificMeeting(int meetingID)
        {
            MeetingRepository mr = new MeetingRepository();
            var meeting = mr.GetMeeting(meetingID);
            var user = mr.GetMeetingCreator(meetingID);
            List<TimeSuggestion> timeList = mr.GetTimeSuggestions(meetingID);
            List<string> times = new List<string>();
            foreach(TimeSuggestion time in timeList)
            {
                times.Add(time.Suggestion);
            }
            var model = new MeetingInvitationsViewModels
            {
                Place = meeting.Place,
                Subject = meeting.Subject,
                Date = meeting.Date,
                Sender = user.FirstName + " " + user.LastName,
                SuggestionsOfTimes = times,
                MeetingID = meetingID
            };
            return PartialView("_SpecificMeetingPartial", model);
        }

        [HttpGet]
        public PartialViewResult MeetingDetails(int meetingID)
        {
            MeetingRepository mr = new MeetingRepository();
            var meeting = mr.GetMeeting(meetingID);
            var user = mr.GetMeetingCreator(meetingID);
            var names = mr.GetMeetingParticipants(meetingID);
            var time = mr.GetConfirmedTime(meetingID);
            var model = new MeetingViewModels
            {
                Subject = meeting.Subject,
                Place = meeting.Place,
                Date = meeting.Date,
                Sender = user.FirstName + " " + user.LastName,
                Names = names,
                Time = time

            };
            return PartialView("_MeetingDetailsPartial", model);
        }

        [HttpGet]
        public PartialViewResult ConfirmMeeting(int meetingID)
        {
            MeetingRepository mr = new MeetingRepository();
            var meeting = mr.GetMeeting(meetingID);
            Dictionary<string, int> times = mr.GetAnsweredTimes(meetingID);
            var model = new ConfirmMeetingViewModels
            {
                Subject = meeting.Subject,
                Place = meeting.Place,
                Date = meeting.Date,
                meetingID = meeting.MID,
                Times = times
            };
            return PartialView("_ConfirmMeetingPartial", model);
        }

        [HttpPost]
        public ActionResult ConfirmMeeting(int meetingID, string time)
        {
            MeetingRepository mr = new MeetingRepository();
            mr.SetMeetingConfirmed(meetingID);
            mr.SetTimeSuggestionConfirmed(meetingID, time);
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