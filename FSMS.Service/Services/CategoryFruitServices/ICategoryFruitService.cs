using FSMS.Service.ViewModels.CategoryFruits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.CategoryFruitServices
{
    public interface ICategoryFruitService
    {
        Task<List<GetCategoryFruit>> GetAllAsync(string? categoryName = null, bool activeOnly = false);
        Task<GetCategoryFruit> GetAsync(int key);
        Task CreateCategoryFruitAsync(CreateCategoryFruit createCategoryFruit);
        Task UpdateCategoryFruitAsync(int key, UpdateCategoryFruit updateCategoryFruit);
        Task DeleteCategoryFruitAsync(int key);
    }
}
