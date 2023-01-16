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
    public class PersonService : Service<PersonCreateDto, PersonUpdateDto, PersonListDto, Person>, IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<DiseaseDoctorCreateDto> _diseaseDoctorCreateValidator;
        int id, titleId;
        string titleName, doctorName;
        
       

        public PersonService(IMapper mapper, IValidator<PersonCreateDto> createDtoValidator, IValidator<PersonUpdateDto> updateDtoValidator, IUnitOfWork unitOfWork, IValidator<DiseaseDoctorCreateDto> diseaseDoctorCreateValidator) : base(mapper, createDtoValidator, updateDtoValidator, unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _diseaseDoctorCreateValidator = diseaseDoctorCreateValidator;
        }

        public async Task<IResponse<List<TitleListDto>>> GetPersonTitleAsync()
        {
            var data = await _unitOfWork.GetRepository<Title>().GetAllAsync();
            var dto = _mapper.Map<List<TitleListDto>>(data);
            return new Response<List<TitleListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<List<TitleListDto>>> GetPersonTitleByPersonTitleAsync(string personTitle)
        {
            var data = await _unitOfWork.GetRepository<Title>().GetAllAsync(x => x.PersonTitle == personTitle);
            var dto = _mapper.Map<List<TitleListDto>>(data);
            return new Response<List<TitleListDto>>(ResponseType.Success, dto);
        }

        public async Task<int> GetTitleId(string personTitle)
        {
            var data = await _unitOfWork.GetRepository<Title>().GetAllAsync(x => x.PersonTitle == personTitle);
            var dto = _mapper.Map<List<TitleListDto>>(data);
            foreach (var item in dto)
            {
                id = item.Id;
            }
            return id;
        }

        public async Task<int> GetDoctorOrPatientId(string name, string surname, string phone, string email)
        {
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.FirstName == name
            && x.LastName == surname
            && x.PhoneNumber == phone
            && x.Email == email);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            foreach (var item in dto)
            {
                id = item.Id;
            }
            return id;
        }

        public async Task<int> GetSelectedDoctorId(string doctorItem)
        {
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.FirstName + " " + x.LastName == doctorItem);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            foreach (var item in dto)
            {
                id = item.Id;
            }
            return id;
        }

        public async Task<string> GetTitleById(int id)
        {
            var data = await _unitOfWork.GetRepository<Title>().GetAllAsync(x => x.Id == id);
            var dto = _mapper.Map<List<TitleListDto>>(data);
            foreach (var item in dto)
            {
                titleName = item.PersonTitle;
            }
            return titleName;
        }

        public async Task<int> GetIdByTitle(string title)
        {
            var data = await _unitOfWork.GetRepository<Title>().GetAllAsync(x => x.PersonTitle == title);
            var dto = _mapper.Map<List<TitleListDto>>(data);
            foreach (var item in dto)
            {
                titleId = item.Id;
            }
            return titleId;
        }

        public async Task<string> GetEmailByIdAsync(int id)
        {
            string email = "";
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.Id == id);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            foreach (var item in dto)
            {
                email = item.Email;
            }
            return email;
        }

        public async Task<string> GetDoctorNameById(int id)
        {
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.Id == id);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            foreach (var item in dto)
            {
                doctorName = item.FirstName + " " + item.LastName;
            }
            return doctorName;
        }

        public async Task<string[]> GetAllByTitleAsync(int personTitleId)
        {
            List<string> doctorList = new List<string>();
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x=>x.PersonTitleId == personTitleId);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            foreach (var item in dto)
            {
                doctorList.Add(item.FirstName + " " + item.LastName);
            }
            return doctorList.ToArray();
        }

        public async Task<double[]> GetAllDoctorIdByNameAsync(string[] personArray)
        {
            List<double> doctorIdList = new List<double>();
            for (int i = 0; i < personArray.Length; i++)
            {
                var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.FirstName + " " + x.LastName == personArray[i]);
                var dto = _mapper.Map<List<PersonListDto>>(data);
                foreach (var item in dto)
                {
                    doctorIdList.Add(item.Id);
                }
            }
            return doctorIdList.ToArray();
        }

        public async Task<IResponse<List<PersonListDto>>> GetActiveAsync(int titleId)
        {
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.Status == true && x.PersonTitleId == titleId);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            return new Response<List<PersonListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<List<PersonListDto>>> GetInActiveAsync(int titleId)
        {
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.Status == false && x.PersonTitleId == titleId);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            return new Response<List<PersonListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<List<PersonListDto>>> GetAllByTitleCountAsync(int titleId)
        {
            var data = await _unitOfWork.GetRepository<Person>().GetAllAsync(x => x.PersonTitleId == titleId);
            var dto = _mapper.Map<List<PersonListDto>>(data);
            return new Response<List<PersonListDto>>(ResponseType.Success, dto);
        }
    }
}
