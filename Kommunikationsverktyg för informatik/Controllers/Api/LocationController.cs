using DataAccess.Models;
using Kommunikationsverktyg_för_informatik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Kommunikationsverktyg_för_informatik.Controllers.Api
{
    public class LocationController : ApiController
    {
        [HttpPost]
        public void Add(LocationViewModel pos)
        {
            using (DataContext db = new DataContext())
            {
                Location loc = new Location()
                {
                    Longitude = pos.Longitude,
                    Latitude = pos.Latitude
                };
                db.Location.Add(loc);
                db.SaveChanges();
            }
        }
    }
}