using FSMS.Entity.Models;
using System.Linq.Expressions;

namespace FSMS.Entity.Repositories.CropVarietyRepositories
{
    public interface ICropVarietyRepository : IRepositoryBase<CropVariety>
    {
        Task<IEnumerable<CropVariety>> GetAllProductWithPlantsAsync(
           Expression<Func<CropVariety, bool>> filter = null,
           Func<IQueryable<CropVariety>, IOrderedQueryable<CropVariety>> orderBy = null,
           string includeProperties = "");
        Task<CropVariety> GetProductByIDAsync(int id);
    }
}
