using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Location
    {
        [ForeignKey("Post")]
        public string Id { get; set; }

        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }
       
        public virtual Post Post { get; set; }

        //public Location()
        //{
        //    Id = Guid.NewGuid();
        //}
    }

}
