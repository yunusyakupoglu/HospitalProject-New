using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OL
{
    public class Appointment : BaseEntity
    {
        public int PatientId { get; set; }
        public int DiseaseId { get; set; }
        public int DoctorId { get; set; }
        public string Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool isActiveAppointment { get; set; }
    }
}
