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
    public class DiseasePatientService : Service<DiseasePatientCreateDto, DiseasePatientUpdateDto, DiseasePatientListDto, DiseasePatient>, IDiseasePatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       
       
        public DiseasePatientService(IMapper mapper, IValidator<DiseasePatientCreateDto> createDtoValidator, IValidator<DiseasePatientUpdateDto> updateDtoValidator, IUnitOfWork unitOfWork) : base(mapper, createDtoValidator, updateDtoValidator, unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetIdByPatientId(int patientId)
        {
            int id=0;
            var data = await _unitOfWork.GetRepository<DiseasePatient>().GetAllAsync(x => x.PersonId == patientId);
            var dto = _mapper.Map<List<DiseasePatientListDto>>(data);
            foreach (var item in dto)
            {
                id = item.Id;
            }
            return id;
        }

        public async Task<double[]> GetPatientCountByDiseaseIdArray(int[] getDiseaseIdArray)
        {
            List<double> diseasePatientIdDoubleList = new List<double>();
            for (int i = 0; i < getDiseaseIdArray.Length; i++)
            {
                var data = await _unitOfWork.GetRepository<DiseasePatient>().GetAllAsync(x => x.DiseaseId == getDiseaseIdArray[i]);
                var dto = _mapper.Map<List<DiseasePatientListDto>>(data);
                diseasePatientIdDoubleList.Add(data.Count);
            }
            return diseasePatientIdDoubleList.ToArray();
        }
    }
}
