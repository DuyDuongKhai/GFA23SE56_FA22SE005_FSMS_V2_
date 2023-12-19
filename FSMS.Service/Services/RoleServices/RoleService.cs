using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.RoleRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.CropVariety;
using FSMS.Service.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _roleProductRepository;
        private IMapper _mapper;
        public RoleService(IRoleRepository roleProductRepository, IMapper mapper)
        {
            _roleProductRepository = roleProductRepository;
            _mapper = mapper;
        }
        public async Task CreateRoleAsync(CreateRole createRole)
        {
            try
            {
                int lastId = (await _roleProductRepository.GetAsync()).Max(x => x.RoleId);
                Role role = new Role()
                {
                    RoleName = createRole.RoleName,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    RoleId = lastId + 1
                };

                await _roleProductRepository.InsertAsync(role);
                await _roleProductRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRoleAsync(int key)
        {
            try
            {
                Role existedRole = await _roleProductRepository.GetByIDAsync(key);

                if (existedRole == null)
                {
                    throw new Exception("Role ID does not exist in the system.");
                }
                if (existedRole.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Role is not active.");
                }
                existedRole.Status = StatusEnums.InActive.ToString();

                await _roleProductRepository.UpdateAsync(existedRole);
                await _roleProductRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task<GetRole> GetAsync(int key)
        {
            try
            {
                Role role = await _roleProductRepository.GetByIDAsync(key);

                if (role == null)
                {
                    throw new Exception("Role ID does not exist in the system.");
                }
                if (role.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Role is not active.");
                }

                GetRole result = _mapper.Map<GetRole>(role);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetRole>> GetAllAsync()
        {
            try
            {
                IEnumerable<Role> roles = await _roleProductRepository.GetAsync();

                List<GetRole> result = roles.Select(role => _mapper.Map<GetRole>(role)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching roles.", ex);
            }
        }





        public async Task UpdateRoleAsync(int key, UpdateRole updateRole)
        {
            try
            {
                Role existedRole = await _roleProductRepository.GetByIDAsync(key);

                if (existedRole == null)
                {
                    throw new Exception("Role ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateRole.RoleName))
                {
                    existedRole.RoleName = updateRole.RoleName;
                }
                if (!string.IsNullOrEmpty(updateRole.Status))
                {
                    if (updateRole.Status != "Active" && updateRole.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedRole.Status = updateRole.Status;
                }

                existedRole.UpdateDate = DateTime.Now;


                await _roleProductRepository.UpdateAsync(existedRole);
                await _roleProductRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
