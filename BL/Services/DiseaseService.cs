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

namespace BL.Services
{
    public class DiseaseService : Service<DiseaseCreateDto, DiseaseUpdateDto, DiseaseListDto, Disease>, IDiseaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       
       
        public DiseaseService(IMapper mapper, IValidator<DiseaseCreateDto> createDtoValidator, IValidator<DiseaseUpdateDto> updateDtoValidator, IUnitOfWork unitOfWork) : base(mapper, createDtoValidator, updateDtoValidator, unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResponse<List<DiseaseListDto>>> GetActiveAsync()
        {
            var data = await _unitOfWork.GetRepository<Disease>().GetAllAsync(x => x.Status == true);
            var dto = _mapper.Map<List<DiseaseListDto>>(data);
            return new Response<List<DiseaseListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<List<DiseaseListDto>>> GetInActiveAsync()
        {
            var data = await _unitOfWork.GetRepository<Disease>().GetAllAsync(x => x.Status == false);
            var dto = _mapper.Map<List<DiseaseListDto>>(data);
            return new Response<List<DiseaseListDto>>(ResponseType.Success, dto);
        }

        public async Task<int> GetDiseaseId(string diseaseItem)
        {
            int id=0;
            var data = await _unitOfWork.GetRepository<Disease>().GetAllAsync(x => x.Name == diseaseItem);
            var dto = _mapper.Map<List<DiseaseListDto>>(data);
            foreach (var item in dto)
            {
                id = item.Id;
            }
            return id;
        }

        public async Task<int[]> GetDiseaseIdArrayByDiseaseNameArray(string[] diseaseItem)
        {
            List<int> idList = new List<int>();
            for (int i = 0; i < diseaseItem.Length; i++)
            {
                var data = await _unitOfWork.GetRepository<Disease>().GetAllAsync(x => x.Name == diseaseItem[i]);
                var dto = _mapper.Map<List<DiseaseListDto>>(data);
                foreach (var item in dto)
                {
                    idList.Add(item.Id);
                }
            }

            return idList.ToArray();
        }

        public async Task<string> GetDiseaseNameById(int diseaseId)
        {
            string diseaseName = string.Empty;
            var data = await _unitOfWork.GetRepository<Disease>().GetAllAsync(x => x.Id == diseaseId);
            var dto = _mapper.Map<List<DiseaseListDto>>(data);
            foreach (var item in dto)
            {
                diseaseName = item.Name;
            }
            return diseaseName;
        }

        public async Task<string[]> GetAllDiseaseNames()
        {
            var data = await _unitOfWork.GetRepository<Disease>().GetAllAsync();
            var dto = _mapper.Map<List<DiseaseListDto>>(data);
            List<string> names = new List<string>();
            foreach (var item in dto)
            {
                names.Add(item.Name);
            }
            return names.ToArray();
        }

    }
}
