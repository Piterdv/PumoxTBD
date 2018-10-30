using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PumoxTBD.Models
{
    public class SearchOptions
    {
        public string Keywords { get; set; }
        public string DateOfBirthFrom { get; set; }
        public string DateOfBirthTo { get; set; }
        public string JobTitles { get; set; }
    }
}