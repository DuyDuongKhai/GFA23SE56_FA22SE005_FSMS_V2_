using FSMS.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FSMS.Entity.Repositories.CropVarietyRepositories
{
    public class CropVarietyRepository : RepositoryBase<CropVariety>, ICropVarietyRepository
    {
        public CropVarietyRepository() { }
        public async Task<IEnumerable<CropVariety>> GetAllProductWithPlantsAsync(
         Expression<Func<CropVariety, bool>> filter = null,
         Func<IQueryable<CropVariety>, IOrderedQueryable<CropVariety>> orderBy = null,
         string includeProperties = "")
        {
            IQueryable<CropVariety> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query
                .AsNoTracking()
                .Include(p => p.Plants)
                .ToListAsync();
        }

        public async Task<CropVariety> GetProductByIDAsync(int id)
        {
            return await context.Set<CropVariety>()
                .AsNoTracking()
                .Include(p => p.Plants)
                .Where(x => x.CropVarietyId == id)
                .FirstOrDefaultAsync();
        }
    }
}
