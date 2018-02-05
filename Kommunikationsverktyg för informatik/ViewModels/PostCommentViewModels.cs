using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class PostCommentViewModels
    {
        public string Content { get; set; }

        public string Author { get; set; }

        public int PostID { get; set; }

        public string ConvertedDateTime { get; set; }
    }
}