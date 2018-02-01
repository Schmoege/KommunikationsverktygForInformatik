﻿using Kommunikationsverktyg_för_informatik.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Meeting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MID { get; set; }

        [MaxLength(50)]
        [Required]
        public string Subject { get; set; }

        [MaxLength(150)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [MaxLength(20)]
        [Required]
        public string Place { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
