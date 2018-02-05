using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Post
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [MaxLength (30)]
        [DisplayName("Titel")]
        public string Title { get; set; }

        [DisplayName("Innehåll")]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string User { get; set; }
        public ApplicationUser ApplicationUser;

        public Guid KategoriId { get; set; }
        public Kategori Kategori;

        public Post()
        {     
                Id = Guid.NewGuid();
        }
        
    }
}