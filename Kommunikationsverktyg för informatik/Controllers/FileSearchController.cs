using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class FileSearchController : Controller
    {
        // GET: FileSearch
        public ActionResult FileSearch()
        {
            var model = new FileSearchViewModel();
            return View(model);
        }

        public ActionResult Search()
        {
            var model = new FileSearchViewModel();
            //if (dateFrom != null && dateTo != null && model != null)
            //{
            //    model.Testing = "Testmeddelande";
            //    return PartialView("_SearchResultPartialView", model);
            //}
            model.Testing = "Testmeddelande";
            return PartialView("_SearchResultPartialView", model);
        }

        [HttpPost]
        public JsonResult getFileSearchResult(string dateFrom, string dateTo)
        {
            DateTime firstDate = DateTime.Parse(dateFrom);
            DateTime secondDate = DateTime.Parse(dateTo).AddDays(1); //+1 dag för att inkludera den sista dagen
            
            List<UserFile> matchedFiles = new List<UserFile>();
            try
            {
                using (var db = new DataContext())
                {
                    var postsBetweenSelectedDates = db.Posts.Where(x => x.Date >= firstDate && x.Date <= secondDate).Select(x => x.Id);

                    matchedFiles.AddRange(db.UserFiles.Where(x => postsBetweenSelectedDates.Contains(x.BlogPostId)));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(matchedFiles);
        }
    }

}