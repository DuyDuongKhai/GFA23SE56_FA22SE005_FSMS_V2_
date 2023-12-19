using FSMS.Service.ViewModels.Roles;
using FSMS.Service.ViewModels.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.RoleServices
{
    public interface IRoleService
    {
        Task<List<GetRole>> GetAllAsync();
        Task<GetRole> GetAsync(int key);
        Task CreateRoleAsync(CreateRole createRole);
        Task UpdateRoleAsync(int key, UpdateRole updateRole);
        Task DeleteRoleAsync(int key);
    }
}
