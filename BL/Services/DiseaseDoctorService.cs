using AutoMapper;
using BL.IServices;
using DAL.Migrations;
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
    public class DiseaseDoctorService : Service<DiseaseDoctorCreateDto, DiseaseDoctorUpdateDto, DiseaseDoctorListDto, DiseaseDoctor>, IDiseaseDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        int diseaseId;
        int diseaseDoctorId;
        List<int> diseaseDoctorIdList = new List<int>();
        List<double> diseaseDoctorIdDoubleList = new List<double>();
        public DiseaseDoctorService(IMapper mapper, IValidator<DiseaseDoctorCreateDto> createDtoValidator, IValidator<DiseaseDoctorUpdateDto> updateDtoValidator, IUnitOfWork unitOfWork) : base(mapper, createDtoValidator, updateDtoValidator, unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetIdByPersonId(int personId)
        {
            var data = await _unitOfWork.GetRepository<DiseaseDoctor>().GetAllAsync(x => x.PersonId == personId);
            var dto = _mapper.Map<List<DiseaseDoctorListDto>>(data);
            foreach (var item in dto)
            {
                diseaseId = item.DiseaseId;
            }
            return diseaseId;
        }

        public async Task<int> GetIdByDoctorId(int personId)
        {
            var data = await _unitOfWork.GetRepository<DiseaseDoctor>().GetAllAsync(x => x.PersonId == personId);
            var dto = _mapper.Map<List<DiseaseDoctorListDto>>(data);
            foreach (var item in dto)
            {
                diseaseDoctorId = item.Id;
            }
            return diseaseDoctorId;
        }
        public async Task<List<int>> GetDoctorIdByDiseaseId(int getDiseaseId)
        {
            var data = await _unitOfWork.GetRepository<DiseaseDoctor>().GetAllAsync(x => x.DiseaseId == getDiseaseId);
            var dto = _mapper.Map<List<DiseaseDoctorListDto>>(data);
            diseaseDoctorIdList = new List<int>();
            foreach (var item in dto)
            {
                diseaseDoctorIdList.Add(item.PersonId);
            }
            return diseaseDoctorIdList;
        }

        public async Task<double[]> GetDoctorCountByDiseaseIdArray(int[] getDiseaseIdArray)
        {
            for (int i = 0; i < getDiseaseIdArray.Length; i++)
            {
                var data = await _unitOfWork.GetRepository<DiseaseDoctor>().GetAllAsync(x => x.DiseaseId == getDiseaseIdArray[i]);
                var dto = _mapper.Map<List<DiseaseDoctorListDto>>(data);
                diseaseDoctorIdDoubleList.Add(data.Count);
            }
            return diseaseDoctorIdDoubleList.ToArray();
        }
    }
}
