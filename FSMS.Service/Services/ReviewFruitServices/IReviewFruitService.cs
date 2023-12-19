using FSMS.Service.ViewModels.ReviewFruits;

namespace FSMS.Service.Services.ReviewFruitServices
{
    public interface IReviewFruitService
    {
        Task<List<GetReviewFruit>> GetAllReviewFruitsAsync(bool activeOnly = false, int? fruitId = null);
        Task<GetReviewFruit> GetAsync(int key);
        Task CreateReviewFruitAsync(CreateReviewFruit createReviewFruit);
        Task UpdateReviewFruitAsync(int key, UpdateReviewFruit updateReviewFruit);
        Task DeleteReviewFruitAsync(int key);
    }
}
