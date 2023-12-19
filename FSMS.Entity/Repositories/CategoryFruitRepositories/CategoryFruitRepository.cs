using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CommentRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Entity.Repositories.CategoryFruitRepositories
{
    public class CategoryFruitRepository : RepositoryBase<CategoryFruit>, ICategoryFruitRepository
    {
        public CategoryFruitRepository() { }
    }
}
