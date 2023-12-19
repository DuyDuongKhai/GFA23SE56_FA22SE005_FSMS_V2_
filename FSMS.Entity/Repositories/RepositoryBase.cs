using FSMS.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FSMS.Entity.Repositories
{
    public class RepositoryBase<TEntity> where TEntity : class
    {
        internal FruitSeasonManagementSystemV10Context context;
        internal DbSet<TEntity> dbSet;
        internal DbSet<Fruit> dbSet1;
        internal DbSet<CropVariety> dbSet2;

        public RepositoryBase()
        {
            context = new FruitSeasonManagementSystemV10Context();
            dbSet = context.Set<TEntity>();
            dbSet1 = context.Set<Fruit>();
            dbSet2 = context.Set<CropVariety>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
         Expression<Func<TEntity, bool>> filter = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
         string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

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

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            await DeleteAsync(entityToDelete);
        }

        public virtual async Task DeleteAsync(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual async Task UpdateAsync(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        public virtual async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(
             Expression<Func<TEntity, bool>> filter = null,
             Expression<Func<TEntity, object>>[] includeProperties = null)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }
            var a = await query.FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

    }
}
