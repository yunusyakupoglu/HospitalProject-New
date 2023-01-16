using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class DiseasePatientCreateDto : IDto
    {
        public int PersonId { get; set; }
        public int DiseaseId { get; set; }
        public bool Status { get; set; }
    }
}
