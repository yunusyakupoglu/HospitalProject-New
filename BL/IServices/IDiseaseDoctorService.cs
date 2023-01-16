using DTOs;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IDiseaseDoctorService : IService<DiseaseDoctorCreateDto, DiseaseDoctorUpdateDto, DiseaseDoctorListDto, DiseaseDoctor>
    {
        Task<int> GetIdByPersonId(int personId);
        Task<int> GetIdByDoctorId(int personId);
        Task<List<int>> GetDoctorIdByDiseaseId(int getDiseaseId);
        Task<double[]> GetDoctorCountByDiseaseIdArray(int[] getDiseaseIdArray);
    }
}
