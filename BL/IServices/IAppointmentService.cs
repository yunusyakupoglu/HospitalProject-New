using Common;
using DTOs;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IAppointmentService : IService<AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto, Appointment>
    {
        Task<int> GetDiseaseIdByPatientId(int patientId);
        Task<int> GetDoctorIdByPatientId(int patientId);
        Task<int> GetIdByPatientId(int patientId);
        Task<IResponse<List<AppointmentListDto>>> GetAllAsync(int doctorId);
        Task<IResponse<AppointmentIsCancelUpdateDto>> UpdateIsCancelAsync(AppointmentIsCancelUpdateDto dto);
        Task<double[]> GetPatientCountByDoctorIdAsync(double[] doctorIds);
        Task<string> GetPatientEmailAddressByAppointmentIdAsync(int appointmentId);
    }
}
