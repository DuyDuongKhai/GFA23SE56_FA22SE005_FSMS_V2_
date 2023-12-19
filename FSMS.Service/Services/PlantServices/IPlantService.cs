using FSMS.Service.ViewModels.Plants;

namespace FSMS.Service.Services.PlantServices
{
    public interface IPlantService
    {
        Task<List<GetPlant>> GetAllAsync(string? cropName = null, DateTime? plantingDate = null, bool activeOnly = false, int gardenId = 0, int userId = 0, int CropVarietyId = 0);
        Task<GetPlant> GetAsync(int key);
        Task<GetPlant> CreatePlantAsync(CreatePlant createPlant);
        Task UpdatePlantAsync(int key, UpdatePlant updatePlant);
        Task DeletePlantAsync(int key);
    }
}
