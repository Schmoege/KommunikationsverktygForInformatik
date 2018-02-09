using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Web;
using System.Web.Mvc;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class FileSearchController : Controller
    {
        // GET: FileSearch
        public ActionResult FileSearch()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getFileSearchResult(string dateFrom, string dateTo)
        {
            SoundPlayer player = new SoundPlayer();
            player.Stop();
            var kaiPath = System.AppDomain.CurrentDomain.BaseDirectory + "Icons\\HEJKAI.wav";
            player.SoundLocation = kaiPath;
            player.Play();

            List<UserFile> matchedFiles = new List<UserFile>();
            try
            {
                DateTime firstDate = DateTime.Parse(dateFrom);
                DateTime secondDate = DateTime.Parse(dateTo); 

                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();
                var dateCompare = DateTime.Compare(firstDate, secondDate); //Om större än 0 är firstDate efter secondDate
                if (dateCompare < 1)
                {
                    date1 = firstDate;
                    date2 = secondDate;
                }
                else
                {
                    date1 = secondDate;
                    date2 = firstDate;
                }
                date2 = date2.AddDays(1);//+1 dag för att inkludera den sista dagen
                using (var db = new DataContext())
                {
                    var postsBetweenSelectedDates = db.Posts.Where(x => x.Date >= date1 && x.Date <= date2).Select(x => x.Id);

                    matchedFiles.AddRange(db.UserFiles.Where(x => postsBetweenSelectedDates.Contains(x.BlogPostId)));
                }
            }
            catch (Exception)
            {
                return Json(matchedFiles);
            }
            return Json(matchedFiles);
        }
    }

}