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
    public interface IDiseaseService : IService<DiseaseCreateDto,DiseaseUpdateDto,DiseaseListDto,Disease>
    {
        Task<IResponse<List<DiseaseListDto>>> GetActiveAsync();
        Task<IResponse<List<DiseaseListDto>>> GetInActiveAsync();
        Task<int> GetDiseaseId(string diseaseItem);
        Task<string> GetDiseaseNameById(int diseaseId);
        Task<string[]> GetAllDiseaseNames();
        Task<int[]> GetDiseaseIdArrayByDiseaseNameArray(string[] diseaseItem);
    }
}
