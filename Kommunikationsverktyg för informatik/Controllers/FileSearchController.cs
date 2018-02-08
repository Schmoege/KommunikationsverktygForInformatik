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
    }
}