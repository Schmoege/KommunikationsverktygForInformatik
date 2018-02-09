using Kommunikationsverktyg_för_informatik;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using Kommunikationsverktyg_för_informatik.Models;

namespace DataAccess.Repositories
{
    public class MeetingRepository
    {
        public DataContext dataContext;

        public MeetingRepository()
        { 

        }
        
        public void AddMeeting(Meeting meeting, Invitation invitation, List<RecieveMeetingInvitation> rminvitelist, List<TimeSuggestion> timeSuggestions, List<TimeAnswer> timeAnswers)
        {
            using (var db = new DataContext())
            {
                try
                {
                    db.Meetings.Add(meeting);
                    db.SaveChanges();
                    invitation.MeetingID = meeting.MID;
                    db.Invitations.Add(invitation);
                    db.SaveChanges();
                    foreach (TimeSuggestion time in timeSuggestions)
                    {
                        time.MeetingID = meeting.MID;
                        db.TimeSuggestions.Add(time);
                        db.SaveChanges();
                    }
                    int meetingID = meeting.MID;
                    List<TimeSuggestion> timeList = new List<TimeSuggestion>();
                    timeList = db.TimeSuggestions.Where(x => x.MeetingID.Equals(meetingID)).ToList();
                    int i = 0;
                    int j = 0;
                    foreach (TimeAnswer timeAnswer in timeAnswers)
                    {

                        timeAnswer.TimeID = timeList[j].TID;
                        db.TimeAnswers.Add(timeAnswer);
                        db.SaveChanges();
                        i++;
                        if(i % timeSuggestions.Count == 0)
                        {
                            j++;
                        }
                    }
                    foreach(RecieveMeetingInvitation rminvite in rminvitelist)
                    {
                        rminvite.InvitationID = invitation.IID;
                        db.RMInvites.Add(rminvite);
                        db.SaveChanges();
                    }
                    //db.SaveChanges();
                }
                catch(DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }
        
        public List<Meeting> GetInvitedMeetings(string userID)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    List<Meeting> meetingList = new List<Meeting>();
                    meetingList = db.Meetings.SqlQuery("select * from dbo.Meetings where MID in (select MeetingID from Invitations where IID in (select InvitationID from RecieveMeetingInvitations where UserID = @p0))", userID).ToList();
                    return meetingList;
                }
                catch
                {
                    return null;
                }
            }
        }
        
        public ApplicationUser GetMeetingCreator(int meetingID)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    var inv = db.Invitations.Single(x => x.MeetingID.Equals(meetingID));
                    var user = db.Users.Single(x => x.Id.Equals(inv.UserID));
                    return user;
                }
                catch
                {
                    return null;
                }
            }
        }
        
        public Meeting GetMeeting(int meetingID)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    Meeting meeting = new Meeting();
                    meeting = db.Meetings.Single(x => x.MID.Equals(meetingID));
                    return meeting;
                }
                catch
                {
                    return null;
                }
            }
        }     
        
        public List<TimeSuggestion> GetTimeSuggestions(int meetingID)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    List<TimeSuggestion> timeList = new List<TimeSuggestion>();
                    timeList = db.TimeSuggestions.Where(x => x.MeetingID.Equals(meetingID)).ToList();
                    return timeList;
                }
                catch
                {
                    return null;
                }
            }
        }   

        public void SetMeetingAnswer(int newAnswer, int meetingID, string userID)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    Invitation inv = db.Invitations.Single(x => x.MeetingID.Equals(meetingID));
                    var answer = db.RMInvites.Single(x => x.InvitationID.Equals(inv.IID) && x.UserID.Equals(userID));
                    answer.Answered = true;
                    answer.Answer = newAnswer;
                    db.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public void SetTimeAnswer(int meetingID, string time, string userID)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    string ud = userID;
                    TimeSuggestion suggestion = db.TimeSuggestions.Single(x => x.MeetingID.Equals(meetingID) && x.Suggestion.Equals(time));
                    var answer = db.TimeAnswers.Single(x => x.UserID.Equals(ud) && x.TimeID.Equals(suggestion.TID));
                    answer.Answered = true;
                    answer.Answer = 1;
                    db.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool GetAnswer(int meetingID, string userID)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    bool answered;
                    var invID = db.Invitations.Single(x => x.MeetingID.Equals(meetingID)).IID;
                    answered = db.RMInvites.Single(x => x.UserID.Equals(userID) && x.InvitationID.Equals(invID)).Answered;
                    return answered;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
