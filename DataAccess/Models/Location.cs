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
        [Key, ForeignKey("Post")]
        public Guid PostId { get; set; }

        public Post Post { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
