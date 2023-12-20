using FSMS.Service.ViewModels.Seasons;

namespace FSMS.Service.Services.SeasonServices
{
    public interface ISeasonService
    {
        Task<List<GetSeason>> GetAllSeasonsAsync(string? seasonName = null, DateTime? startDate = null, bool activeOnly = false, int? gardenId = null);
        Task<GetSeason> GetAsync(int key);
        Task CreateSeasonAsync(CreateSeason createSeason);
        Task UpdateSeasonAsync(int key, UpdateSeason updateSeason);
        Task DeleteSeasonAsync(int key);

    }
}
