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

        [Required]
        [MaxLength (30)]
        [DisplayName("Titel")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Innehåll")]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string User { get; set; }
        public ApplicationUser ApplicationUser;

        [Required]
        public Guid KategoriId { get; set; }
        public Kategori Kategori;

        public virtual ICollection<ApplicationUser> AppUsers { get; set; }

        public Post()
        {
            Id = Guid.NewGuid();
            this.AppUsers = new HashSet<ApplicationUser>();
        }

    }
}