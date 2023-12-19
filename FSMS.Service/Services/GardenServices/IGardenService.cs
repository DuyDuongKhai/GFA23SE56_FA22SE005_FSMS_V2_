using FSMS.Service.ViewModels.Gardens;

namespace FSMS.Service.Services.GardenServices
{
    public interface IGardenService
    {
        Task<List<GetGarden>> GetAllAsync(string? gardenName = null, bool activeOnly = false, int? userId = null);
        Task<GetGarden> GetAsync(int key);
        Task CreateGardenAsync(CreateGarden createGarden);
        Task UpdateGardenAsync(int key, UpdateGarden updateGarden);
        Task DeleteGardenAsync(int key);
    }
}
