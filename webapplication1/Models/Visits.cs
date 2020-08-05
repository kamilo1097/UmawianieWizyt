using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Visits
    {
        public int Id { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        [Display(Name = "Imie")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Numer telefonu")]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data wizyty")]
        public DateTime Date { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + ", " + LastName;
            }
        }
    }
}
