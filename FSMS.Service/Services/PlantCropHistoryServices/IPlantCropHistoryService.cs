using FSMS.Service.ViewModels.PlantCropHistory;

namespace FSMS.Service.Services.PlantCropHistoryServices
{
    public interface IPlantCropHistoryService
    {
        Task<List<GetPlantCropHistory>> GetAllAsync(string? gardenName = null, string? plantName = null, string? seasonName = null);

        Task CreatePlantCropHistoryAsync(CreatePlantCropHistory createPlantCropHistory);

    }
}
