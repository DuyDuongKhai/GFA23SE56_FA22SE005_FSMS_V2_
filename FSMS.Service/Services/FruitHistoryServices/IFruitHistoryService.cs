using FSMS.Service.ViewModels.FruitHistories;

namespace FSMS.Service.Services.FruitHistoryServices
{
    public interface IFruitHistoryService
    {
        Task<List<GetFruitHistory>> GetAllAsync(string? location = null, DateTime? createdDate = null, int userId = 0);
        Task ScrapeFarmerMarketAndSaveToDatabaseAsync(int userId);
        Task ScrapeThucPhamNhanhAndSaveToDatabaseAsync(int userId);
        Task DeleteFruitHistoryAsync(int userId);


    }
}
