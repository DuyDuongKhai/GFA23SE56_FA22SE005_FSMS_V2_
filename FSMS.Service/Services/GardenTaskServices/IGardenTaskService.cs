using FSMS.Service.ViewModels.GardenTasks;

namespace FSMS.Service.Services.GardenTaskServices
{
    public interface IGardenTaskService
    {
        Task<List<GetGardenTask>> GetAllAsync(string? gardenTaskName = null, DateTime? taskDate = null, bool activeOnly = false, int gardenId = 0, int plantId = 0);
        Task<GetGardenTask> GetAsync(int key);
        Task CreateGardenTaskAsync(CreateGardenTask createGardenTask);
        Task UpdateGardenTaskAsync(int key, UpdateGardenTask updateGardenTask);
        Task DeleteGardenTaskAsync(int key);
    }
}
