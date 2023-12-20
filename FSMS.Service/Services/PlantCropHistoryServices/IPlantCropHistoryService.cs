using FSMS.Service.ViewModels.PlantCropHistory;

namespace FSMS.Service.Services.PlantCropHistoryServices
{
    public interface IPlantCropHistoryService
    {
        Task<List<GetPlantCropHistory>> GetAllAsync(string? gardenName = null, string? plantName = null, string? seasonName = null, int? gardenId = null, int? plantId = null, int? seasonId = null);

        Task CreatePlantCropHistoryAsync(CreatePlantCropHistory createPlantCropHistory);

        Task<List<PlantHarvestStats>> GetPlantHarvestStatsAsync(string? plantName, int plantId);

    }
}
