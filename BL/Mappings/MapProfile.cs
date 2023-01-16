using AutoMapper;
using DTOs;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<PersonCreateDto,Person>().ReverseMap();
            CreateMap<PersonUpdateDto,Person>().ReverseMap();
            CreateMap<PersonListDto,Person>().ReverseMap();

            CreateMap<TitleListDto,Title>().ReverseMap();

            CreateMap<DiseaseDoctorCreateDto,DiseaseDoctor>().ReverseMap();
            CreateMap<DiseaseDoctorUpdateDto,DiseaseDoctor>().ReverseMap();
            CreateMap<DiseaseDoctorListDto,DiseaseDoctor>().ReverseMap();

            CreateMap<DiseasePatientCreateDto, DiseasePatient>().ReverseMap();
            CreateMap<DiseasePatientUpdateDto, DiseasePatient>().ReverseMap();
            CreateMap<DiseasePatientListDto, DiseasePatient>().ReverseMap();

            CreateMap<DiseaseCreateDto, Disease>().ReverseMap();
            CreateMap<DiseaseUpdateDto, Disease>().ReverseMap();
            CreateMap<DiseaseListDto, Disease>().ReverseMap();

            CreateMap<AppointmentCreateDto, Appointment>().ReverseMap();
            CreateMap<AppointmentUpdateDto, Appointment>().ReverseMap();
            CreateMap<AppointmentIsCancelUpdateDto, Appointment>().ReverseMap();
            CreateMap<AppointmentListDto, Appointment>().ReverseMap();

            CreateMap<AppointmentNoteCreateDto, AppointmentNote>().ReverseMap();
            CreateMap<AppointmentNoteUpdateDto, AppointmentNote>().ReverseMap();
            CreateMap<AppointmentNoteListDto, AppointmentNote>().ReverseMap();
        }
    }
}
