using Kommunikationsverktyg_för_informatik;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;

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
                    int i = 0;
                    foreach(TimeAnswer timeAnswer in timeAnswers)
                    {
                        timeAnswer.TimeID = timeSuggestions[i].TID;
                        db.TimeAnswers.Add(timeAnswer);
                        db.SaveChanges();
                        i++;
                        i = i % timeSuggestions.Count;
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
    }
}
