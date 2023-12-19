using FSMS.Service.Services.GardenServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations;
using FSMS.Service.Validations.Garden;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Gardens;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/gardens")]
    [ApiController]
    public class GardensController : ControllerBase
    {
        private IGardenService _gardenService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public GardensController(IGardenService gardenService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _gardenService = gardenService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAllGardens(string? gardenName = null, bool activeOnly = false, int? userId = null)
        {
            try
            {
                List<GetGarden> gardens = await _gardenService.GetAllAsync(gardenName, activeOnly, userId);
                gardens = gardens.OrderByDescending(c => c.CreatedDate).ToList();

                return Ok(new
                {
                    Data = gardens
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
        public async Task<IActionResult> GetGardenById(int id)
        {
            try
            {
                GetGarden garden = await _gardenService.GetAsync(id);
                return Ok(new
                {
                    Data = garden
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
        public async Task<IActionResult> CreateGarden([FromForm] CreateGarden createGarden)
        {
            var validator = new GardenValidator();
            var validationResult = validator.Validate(createGarden);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _gardenService.CreateGardenAsync(createGarden);

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
        public async Task<IActionResult> UpdateGarden(int id, [FromForm] UpdateGarden updateGarden)
        {
            var validator = new UpdateGardenValidator();
            var validationResult = validator.Validate(updateGarden);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _gardenService.UpdateGardenAsync(id, updateGarden);
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
        public async Task<IActionResult> DeleteGarden(int id)
        {
            try
            {
                await _gardenService.DeleteGardenAsync(id);
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
