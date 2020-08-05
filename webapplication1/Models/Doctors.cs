using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Doctors
    {
        public int Id { get; set; }
        public string PersonalData { get; set; }
        [ForeignKey("Specialization")]
        public int SpecializationId { get; set; }
        public Specializations Specialization { get; set; }

        public List<Visits> Visits { get; set; }
    }
}
