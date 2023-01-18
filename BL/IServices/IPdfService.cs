using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IPdfService
    {
        Task<bool> GenerateAsync(string patientNameSurname, string doctorName, string appointmentDate, string appointmentTime, string note);
    }
}
