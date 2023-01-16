using DTOs;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IAppointmentNoteService : IService<AppointmentNoteCreateDto, AppointmentNoteUpdateDto, AppointmentNoteListDto, AppointmentNote>
    {
        Task<string> GetNoteByAppointmentId(int appointmentId);
    }
}
