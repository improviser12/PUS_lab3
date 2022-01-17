using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUS_lab3.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Year { get; set; }
    }
}
