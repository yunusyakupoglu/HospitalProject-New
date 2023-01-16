using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class DiseaseCreateDto : IDto
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
