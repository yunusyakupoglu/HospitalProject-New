using AutoMapper;
using BL.IServices;
using DAL.UnitOfWorks;
using DTOs;
using FluentValidation;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class AppointmentNoteService : Service<AppointmentNoteCreateDto, AppointmentNoteUpdateDto, AppointmentNoteListDto, AppointmentNote>, IAppointmentNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AppointmentNoteService(IMapper mapper, IValidator<AppointmentNoteCreateDto> createDtoValidator, IValidator<AppointmentNoteUpdateDto> updateDtoValidator, IUnitOfWork unitOfWork) : base(mapper, createDtoValidator, updateDtoValidator, unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> GetNoteByAppointmentId(int appointmentId)
        {
            var data = await _unitOfWork.GetRepository<AppointmentNote>().GetAllAsync(x => x.AppointmentId == appointmentId);
            var dto = _mapper.Map<List<AppointmentNoteListDto>>(data);
            if (data.Count != 0)
            {
                return dto[0].Note;
            }
            return string.Empty;
        }
    }
}
