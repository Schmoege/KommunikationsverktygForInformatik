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
        public void Set(LocationViewModel loc)
        {
            using (DataContext db = new DataContext())
            {
                Location location = new Location()
                {
                    Latitude = loc.lat,
                    Longitude = loc.lng,
                    PostId = new Guid(loc.PostId)
                };
                db.Location.Add(location);
                db.SaveChanges();
            }
        }
    }
}