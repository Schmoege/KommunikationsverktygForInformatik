using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kommunikationsverktyg_för_informatik.Controllers
{
    public class ValidationController : Controller
    {
        public JsonResult IsEmailAvailable(string email)
        {
            UserRepository ur = new UserRepository();
            bool available = ur.CheckEmailAvailable(email);
            if (available)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Email not available", JsonRequestBehavior.AllowGet);
            }
        }
    }
}