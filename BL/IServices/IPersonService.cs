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
    public interface IPersonService : IService<PersonCreateDto,PersonUpdateDto,PersonListDto,Person>
    {
        Task<int> Login(string firstName, string lastName);
        Task<IResponse<List<TitleListDto>>> GetPersonTitleAsync();
        Task<IResponse<List<TitleListDto>>> GetPersonTitleByPersonTitleAsync(string personTitle);
        Task<int> GetTitleId(string personTitle);
        Task<string> GetTitleById(int id);
        Task<string> GetDoctorNameById(int id);
        Task<int> GetDoctorOrPatientId(string name, string surname, string phone, string email);
        Task<int> GetSelectedDoctorId(string doctorItem);
        Task<string[]> GetAllByTitleAsync(int personTitleId);
        Task<double[]> GetAllDoctorIdByNameAsync(string[] personArray);
        Task<int> GetIdByTitle(string title);
        Task<IResponse<List<PersonListDto>>> GetActiveAsync(int titleId);
        Task<IResponse<List<PersonListDto>>> GetInActiveAsync(int titleId);
        Task<IResponse<List<PersonListDto>>> GetAllByTitleCountAsync(int titleId);
        Task<string> GetEmailByIdAsync(int id);
    }
}
