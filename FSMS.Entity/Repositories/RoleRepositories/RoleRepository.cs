using FSMS.Entity.Models;
using FSMS.Entity.Repositories.SeasonRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Entity.Repositories.RoleRepositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository() { }
    }
}
