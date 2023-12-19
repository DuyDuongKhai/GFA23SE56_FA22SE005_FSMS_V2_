using FSMS.Service.Services.PlantServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations.Plant;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Plants;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/plants")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private IPlantService _plantService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public PlantsController(IPlantService plantService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _plantService = plantService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet("plants")]
        [Cache(1000)]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAllPlants(string? cropName = null, DateTime? plantingDate = null, bool activeOnly = false, int gardenId = 0, int userId = 0, int CropVarietyId = 0)
        {
            try
            {
                List<GetPlant> plants = await _plantService.GetAllAsync(cropName, plantingDate, activeOnly, gardenId, userId, CropVarietyId);
                plants = plants.OrderByDescending(c => c.CreatedDate).ToList();

                return Ok(new
                {
                    Data = plants
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
        public async Task<IActionResult> GetPlantById(int id)
        {
            try
            {
                GetPlant plant = await _plantService.GetAsync(id);
                return Ok(new
                {
                    Data = plant
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
        public async Task<IActionResult> CreatePlant([FromForm] CreatePlant createPlant)
        {
            var validator = new PlantValidator();
            var validationResult = validator.Validate(createPlant);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _plantService.CreatePlantAsync(createPlant);

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
        public async Task<IActionResult> UpdatePlant(int id, [FromForm] UpdatePlant updatePlant)
        {
            var validator = new UpdatePlantValidator();
            var validationResult = validator.Validate(updatePlant);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _plantService.UpdatePlantAsync(id, updatePlant);
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
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                await _plantService.DeletePlantAsync(id);
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
