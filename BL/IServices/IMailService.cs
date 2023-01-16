using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IMailService
    {
        Task<bool> SendAsync(string patientNameSurname, string doctorName, string appointmentDate, string appointmentTime, string note, string mailAdress);
    }
}
