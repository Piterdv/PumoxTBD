using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PumoxTBD.Models
{
    public class Company
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int EstablishmentYear { get; set; }
        //to powoduje niemożność GET api/Emploees serializacji - trzeba dorzucuć DTO!
        //ale GET api/Companies  serializuje się ok!!!
        public ICollection<Employee> Employees { get; set; }
    }
}