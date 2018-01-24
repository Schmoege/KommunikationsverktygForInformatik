using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string UserName { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        //category
    }
}
