using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PumoxTBD.Models
{

    enum JobTitle { Administrator, Developer, Architect, Manager }

    public class Employee
    {
        public long Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public System.DateTime DateOfBirth { get; set; }
        [Required]
        public string JobTitle { get; set; }
        //Foreign Key
        public long CompanyId { get; set; }
        //powiązane - to od strony company raczej:)
        //public Company Company { get; set; }
    }
}