using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class AppointmentNoteCreateDto : IDto
    {
        public int AppointmentId { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; } = true;
    }
}
