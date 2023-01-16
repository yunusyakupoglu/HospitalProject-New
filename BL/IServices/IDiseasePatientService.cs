using DTOs;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IDiseasePatientService : IService<DiseasePatientCreateDto, DiseasePatientUpdateDto, DiseasePatientListDto, DiseasePatient>
    {
        Task<int> GetIdByPatientId(int patientId);
        Task<double[]> GetPatientCountByDiseaseIdArray(int[] getDiseaseIdArray);
    }
}
