using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PumoxTBD.Models
{
    public class EmployeeDTO
    {
        public long Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        //FK
        //public long CompanyId { get; set; }
        //powiązane
        //public Company Company { get; set; }
    }
}