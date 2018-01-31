using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class UserFile
    {
        [Key]
        public Guid FileID { get; set; }
        public byte[] FileBytes { get; set; }
        public String FileName { get; set; }
        public Post BlogPost { get; set; }
        public int BlogPostId { get; set; }
        public string FileExtension { get; set; }
    }
}

