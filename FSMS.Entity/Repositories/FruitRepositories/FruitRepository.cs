using FSMS.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Entity.Repositories.FruitRepositories
{
    public class FruitRepository : RepositoryBase<Fruit>, IFruitRepository
    {
        public FruitRepository() { }
        public async Task<IEnumerable<Fruit>> GetAllProductAsync(
                   Expression<Func<Fruit, bool>> filter = null,
                   Func<IQueryable<Fruit>, IOrderedQueryable<Fruit>> orderBy = null,
                   string includeProperties = "")
        {
            IQueryable<Fruit> query = dbSet;

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
                .Include(p => p.FruitImages)
                .ToListAsync();
        }

        public async Task<Fruit> GetProductByIDAsync(int id)
        {
            return await context.Set<Fruit>()
                .AsNoTracking()
                .Include(p => p.FruitImages)
                .Include(p => p.Plant)   // Include Crop navigation property
                .Include(p => p.CategoryFruit)   // Include Category navigation property
                .Include(p => p.User)   // Include User navigation property
                .Where(x => x.FruitId == id)
                .FirstOrDefaultAsync();
        }
    }
}
