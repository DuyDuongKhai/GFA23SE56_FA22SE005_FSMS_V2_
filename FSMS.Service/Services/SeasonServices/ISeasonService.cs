using FSMS.Service.ViewModels.Plants;
using FSMS.Service.ViewModels.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.SeasonServices
{
    public interface ISeasonService
    {
        Task<List<GetSeason>> GetAllSeasonsAsync(string? seasonName = null, DateTime? startDate = null, bool activeOnly = false);
        Task<GetSeason> GetAsync(int key);
        Task CreateSeasonAsync(CreateSeason createSeason);
        Task UpdateSeasonAsync(int key, UpdateSeason updateSeason);
        Task DeleteSeasonAsync(int key);
      
    }
}
