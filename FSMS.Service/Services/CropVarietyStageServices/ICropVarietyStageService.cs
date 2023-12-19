using FSMS.Service.ViewModels.CropVarietyStages;

namespace FSMS.Service.Services.CropVarietyStageServices
{
    public interface ICropVarietyStageService
    {
        Task<List<GetCropVarietyStage>> GetAllAsync(string? stageName = null, DateTime? startDate = null, bool activeOnly = false, int? CropVarietyId = null);
        Task<GetCropVarietyStage> GetAsync(int key);
        Task CreateCropVarietyStageAsync(CreateCropVarietyStage createCropVarietyStage);
        Task UpdateCropVarietyStageAsync(int key, UpdateCropVarietyStage updateCropVarietyStage);
        Task DeleteCropVarietyStageAsync(int key);
    }
}
