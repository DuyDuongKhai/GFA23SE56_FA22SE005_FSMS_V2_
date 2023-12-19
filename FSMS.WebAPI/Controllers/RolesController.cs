using FSMS.Service.Services.RoleServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Roles;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public RolesController(IRoleService roleService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _roleService = roleService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                List<GetRole> roles = await _roleService.GetAllAsync();
                return Ok(new
                {
                    Data = roles
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Cache(1000)]

        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                GetRole role = await _roleService.GetAsync(id);
                return Ok(new
                {
                    Data = role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPost]

        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRole createRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _roleService.CreateRoleAsync(createRole);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{id}")]

        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRole updateRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _roleService.UpdateRoleAsync(id, updateRole);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]

        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
