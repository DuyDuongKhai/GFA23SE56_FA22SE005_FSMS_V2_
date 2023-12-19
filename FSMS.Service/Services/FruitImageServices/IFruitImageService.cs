using FSMS.Service.ViewModels.FruitImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.FruitImageServices
{
    public interface IFruitImageService
    {
        Task<List<GetFruitImage>> GetAllAsync();
        Task<GetFruitImage> GetAsync(int key);
        Task/*<List<GetProductImage>>*/ CreateFruitImageAsync(CreateFruitImage createFruitImage);
        Task UpdateFruitImageAsync(int key, UpdateFruitImage updateFruitImage);
        Task DeleteFruitImageAsync(int key);
    }
}
