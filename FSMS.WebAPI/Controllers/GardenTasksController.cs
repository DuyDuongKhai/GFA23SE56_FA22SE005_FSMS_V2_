using FSMS.Service.Services.GardenTaskServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations.GardenTask;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.GardenTasks;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/garden-tasks")]
    [ApiController]
    public class GardenTasksController : ControllerBase
    {
        private IGardenTaskService _gardenTaskService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public GardenTasksController(IGardenTaskService gardenTaskService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _gardenTaskService = gardenTaskService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAllGardenTasks(string? gardenTaskName = null, DateTime? taskDate = null, bool activeOnly = false, int gardenId = 0, int plantId = 0)
        {
            try
            {
                List<GetGardenTask> gardenTasks = await _gardenTaskService.GetAllAsync(gardenTaskName, taskDate, activeOnly, gardenId, plantId);
                return Ok(new
                {
                    Data = gardenTasks
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

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetGardenTaskById(int id)
        {
            try
            {
                GetGardenTask gardenTask = await _gardenTaskService.GetAsync(id);
                return Ok(new
                {
                    Data = gardenTask
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

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> CreateGardenTask([FromForm] CreateGardenTask createGardenTask)
        {
            var validator = new GardentTaskValidator();
            var validationResult = validator.Validate(createGardenTask);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _gardenTaskService.CreateGardenTaskAsync(createGardenTask);

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

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> UpdateGardenTask(int id, [FromForm] UpdateGardenTask updateGardenTask)
        {
            var validator = new UpdateGardentTaskValidator();
            var validationResult = validator.Validate(updateGardenTask);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _gardenTaskService.UpdateGardenTaskAsync(id, updateGardenTask);
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

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> DeleteGardenTask(int id)
        {
            try
            {
                await _gardenTaskService.DeleteGardenTaskAsync(id);
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
