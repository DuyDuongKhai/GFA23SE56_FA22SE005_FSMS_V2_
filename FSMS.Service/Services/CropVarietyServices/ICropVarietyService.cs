using FSMS.Service.ViewModels.CropVariety;

namespace FSMS.Service.Services.CropVarietyServices
{
    public interface ICropVarietyService
    {
        Task<List<GetCropVariety>> GetAllWithPlantsAsync(string? varietyName = null, bool activeOnly = false, int? plantId = null);
        Task<GetCropVariety> GetAsync(int key);
        Task CreateCropVarietyAsync(CreateCropVariety createCropVariety);
        Task UpdateCropVarietyAsync(int key, UpdateCropVariety updateCropVariety);
        Task DeleteCropVarietyAsync(int key);
    }
}
