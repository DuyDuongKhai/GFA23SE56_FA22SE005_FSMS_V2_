using FSMS.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Entity.Repositories.FruitRepositories
{
    public interface IFruitRepository : IRepositoryBase<Fruit>
    {
        Task<IEnumerable<Fruit>> GetAllProductAsync(
            Expression<Func<Fruit, bool>> filter = null,
            Func<IQueryable<Fruit>, IOrderedQueryable<Fruit>> orderBy = null,
            string includeProperties = "");
        Task<Fruit> GetProductByIDAsync(int id);
    }
}
