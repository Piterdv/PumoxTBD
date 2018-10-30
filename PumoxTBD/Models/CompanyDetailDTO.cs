using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PumoxTBD.Models
{
    public class CompanyDetailDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        //employee
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public System.DateTime DateOfBirth { get; set; }
        //public string JobTitle { get; set; }

        //to powoduje niemożność GET api/Emploees serializacji - trzeba dorzucuć DTO!
        //ale GET api/Companies  serializuje się ok!!!
        //public ICollection<EmployeeDTO> Employees { get; set; }
        public List<Employee> Employees { get; set; }
    }
}