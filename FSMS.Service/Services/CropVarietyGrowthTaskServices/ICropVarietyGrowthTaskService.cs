using FSMS.Service.ViewModels.CropVarietyGrowthTasks;

namespace FSMS.Service.Services.CropVarietyGrowthTaskServices
{
    public interface ICropVarietyGrowthTaskService
    {
        Task<List<GetCropVarietyGrowthTask>> GetAllAsync(string? taskName = null, DateTime? startDate = null, bool activeOnly = false, int? cropVarietyStageId = null);
        Task<GetCropVarietyGrowthTask> GetAsync(int key);
        Task CreateCropVarietyGrowthTaskAsync(CreateCropVarietyGrowthTask createCropVarietyGrowthTask);
        Task UpdateCropVarietyGrowthTaskAsync(int key, UpdateCropVarietyGrowthTask updateCropVarietyGrowthTask);
        Task DeleteCropVarietyGrowthTaskAsync(int key);
    }
}
