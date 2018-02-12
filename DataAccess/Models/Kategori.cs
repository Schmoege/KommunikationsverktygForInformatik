using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Kategori
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public string Namn { get; set; }
        public bool Formell { get; set; }

        public Kategori()
        {
            Id = Guid.NewGuid();
        }


    }
}