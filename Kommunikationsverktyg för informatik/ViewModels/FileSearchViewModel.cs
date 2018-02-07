using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kommunikationsverktyg_för_informatik.ViewModels
{
    public class FileSearchViewModel
    {
        public List<UserFile> ReturnFiles { get; set; }
        public string Testing { get; set; }
        public FileSearchViewModel()
        {
            this.ReturnFiles = new List<UserFile>();
            this.Testing = "default";
        }
    }
}