using FSMS.Service.ViewModels.FruitDiscounts;

namespace FSMS.Service.Services.FruitDiscountSevices
{
    public interface IFruitDiscountService
    {
        Task<List<GetFruitDiscount>> GetAllAsync(string? discountName = null, DateTime? discountExpiryDate = null, bool activeOnly = false, int? userId = null, int? fruitId = null);
        Task<GetFruitDiscount> GetAsync(int key);
        Task CreateFruitDiscountAsync(CreateFruitDiscount createFruitDiscount);
        Task UpdateFruitDiscountAsync(int key, UpdateFruitDiscount updateFruitDiscount);
        Task DeleteFruitDiscountAsync(int key);
    }
}
