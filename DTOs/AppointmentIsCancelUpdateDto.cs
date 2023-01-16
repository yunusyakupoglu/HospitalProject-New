using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class AppointmentIsCancelUpdateDto : IUpdateDto
    {
        public int Id { get; set; }
        public bool isActiveAppointment { get; set; }
    }
}
