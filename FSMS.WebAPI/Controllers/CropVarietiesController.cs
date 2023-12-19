using FSMS.Service.Services.CropVarietyServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.CropVariety;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{

    [Route("api/crop-varieties")]
    [ApiController]
    public class CropVarietiesController : ControllerBase
    {
        private ICropVarietyService _cropVarietyService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public CropVarietiesController(ICropVarietyService cropVarietyService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _cropVarietyService = cropVarietyService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Farmer", "Admin")]
        public async Task<IActionResult> GetAllCropVarieties(string? varietyName = null, bool activeOnly = false, int? plantId = null)
        {
            try
            {
                List<GetCropVariety> cropVarieties = await _cropVarietyService.GetAllWithPlantsAsync(varietyName, activeOnly, plantId);

                cropVarieties = cropVarieties.OrderByDescending(c => c.CreatedDate).ToList();

                return Ok(new
                {
                    Data = cropVarieties
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
        [PermissionAuthorize("Farmer", "Admin")]
        public async Task<IActionResult> GetCropVarietyById(int id)
        {
            try
            {
                GetCropVariety cropVariety = await _cropVarietyService.GetAsync(id);
                return Ok(new
                {
                    Data = cropVariety
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
        [PermissionAuthorize("Farmer", "Admin")]
        public async Task<IActionResult> CreateCropVariety([FromForm] CreateCropVariety createCropVariety)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _cropVarietyService.CreateCropVarietyAsync(createCropVariety);

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
        [PermissionAuthorize("Farmer", "Admin")]
        public async Task<IActionResult> UpdateCropVariety(int id, [FromForm] UpdateCropVariety updateCropVariety)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _cropVarietyService.UpdateCropVarietyAsync(id, updateCropVariety);
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
        [PermissionAuthorize("Farmer", "Admin")]
        public async Task<IActionResult> DeleteCropVariety(int id)
        {
            try
            {
                await _cropVarietyService.DeleteCropVarietyAsync(id);
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
