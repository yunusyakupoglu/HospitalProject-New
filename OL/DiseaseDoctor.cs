using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OL
{
    public class DiseaseDoctor : BaseEntity
    {
        public int PersonId { get; set; }
        public int DiseaseId { get; set; }
    }
}
