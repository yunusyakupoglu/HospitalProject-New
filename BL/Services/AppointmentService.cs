using AutoMapper;
using BL.IServices;
using Common.Enums;
using Common;
using DAL.UnitOfWorks;
using DTOs;
using FluentValidation;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.CustomErrorHandling;

namespace BL.Services
{
    public class AppointmentService : Service<AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto, Appointment>, IAppointmentService
    {
        private readonly IPersonService _personService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<AppointmentIsCancelUpdateDto> _appointmentIsCancelUpdateValidator;
        int diseaseId, doctorId, id;

        public AppointmentService(IMapper mapper, IValidator<AppointmentCreateDto> createDtoValidator, IValidator<AppointmentUpdateDto> updateDtoValidator, IUnitOfWork unitOfWork, IValidator<AppointmentIsCancelUpdateDto> appointmentIsCancelUpdateValidator, IPersonService personService) : base(mapper, createDtoValidator, updateDtoValidator, unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appointmentIsCancelUpdateValidator = appointmentIsCancelUpdateValidator;
            _personService = personService;
        }

        public async Task<int> GetDiseaseIdByPatientId(int patientId)
        {
            var data = await _unitOfWork.GetRepository<Appointment>().GetAllAsync(x => x.PatientId == patientId);
            var dto = _mapper.Map<List<AppointmentListDto>>(data);
            foreach (var item in dto)
            {
                diseaseId = item.DiseaseId;
            }
            return diseaseId;
        }

        public async Task<int> GetDoctorIdByPatientId(int patientId)
        {
            var data = await _unitOfWork.GetRepository<Appointment>().GetAllAsync(x => x.PatientId == patientId);
            var dto = _mapper.Map<List<AppointmentListDto>>(data);
            foreach (var item in dto)
            {
                doctorId = item.DoctorId;
            }
            return doctorId;
        }

        public async Task<int> GetIdByPatientId(int patientId)
        {
            var data = await _unitOfWork.GetRepository<Appointment>().GetAllAsync(x => x.PatientId == patientId);
            var dto = _mapper.Map<List<AppointmentListDto>>(data);
            foreach (var item in dto)
            {
                id = item.Id;
            }
            return id;
        }

        public async Task<IResponse<List<AppointmentListDto>>> GetAllAsync(int doctorId)
        {
            var data = await _unitOfWork.GetRepository<Appointment>().GetAllAsync(x => x.DoctorId == doctorId);
            var dto = _mapper.Map<List<AppointmentListDto>>(data);
            return new Response<List<AppointmentListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<AppointmentIsCancelUpdateDto>> UpdateIsCancelAsync(AppointmentIsCancelUpdateDto dto)
        {
            var result = _appointmentIsCancelUpdateValidator.Validate(dto);
            if (result.IsValid)
            {
                var unchangedData = await _unitOfWork.GetRepository<Appointment>().FindAsync(dto.Id);
                if (unchangedData == null)
                    return new Response<AppointmentIsCancelUpdateDto>($"{dto.Id} ye sahip data bulunamadı", ResponseType.NotFound);
                var entity = _mapper.Map<Appointment>(dto);
                entity.Date = unchangedData.Date;
                entity.DiseaseId = unchangedData.DiseaseId;
                entity.DoctorId = unchangedData.DoctorId;
                entity.PatientId = unchangedData.PatientId;
                entity.Hour = unchangedData.Hour;
                entity.Minute = unchangedData.Minute;
                entity.Status = unchangedData.Status;
                _unitOfWork.GetRepository<Appointment>().Update(entity, unchangedData);
                await _unitOfWork.SaveChangesAsync();
                return new Response<AppointmentIsCancelUpdateDto>(ResponseType.Success, dto);
            }
            return new Response<AppointmentIsCancelUpdateDto>(dto, result.ConvertToCustomValidationError());
        }

        public async Task<double[]> GetPatientCountByDoctorIdAsync(double[] doctorIds)
        {
            List<double> patientCount = new List<double>();
            for (int i = 0; i < doctorIds.Length; i++)
            {
                var data = await _unitOfWork.GetRepository<Appointment>().GetAllAsync(x => x.DoctorId == doctorIds[i]);
                var dto = _mapper.Map<List<AppointmentListDto>>(data);
                patientCount.Add(dto.Count);
            }
            return patientCount.ToArray();
        }

        public async Task<string> GetPatientEmailAddressByAppointmentIdAsync(int appointmentId)
        {
            string emailAddress = string.Empty;
            var data = await _unitOfWork.GetRepository<Appointment>().GetAllAsync(x => x.Id == appointmentId);
            var dto = _mapper.Map<List<AppointmentListDto>>(data);
            foreach (var item in dto)
            {
                emailAddress = await _personService.GetEmailByIdAsync(id: item.PatientId);
            }
            return emailAddress;
        }
    }
}
