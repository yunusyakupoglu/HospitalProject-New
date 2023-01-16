using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OL
{
    public class AppointmentNote : BaseEntity
    {
        public int AppointmentId { get; set; }
        public string Note { get; set; }
    }
}
