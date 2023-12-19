using FSMS.Service.Services.PlantCropHistoryServices;
using FSMS.Service.Utility;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.PlantCropHistory;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/plant-crop-hitories")]
    [ApiController]
    public class PlantCropHistoriesController : ControllerBase
    {
        private IPlantCropHistoryService _plantCropHistoryService;
        private IOptions<JwtAuth> _jwtAuthOptions;


        public PlantCropHistoriesController(IPlantCropHistoryService plantCropHistoryService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _plantCropHistoryService = plantCropHistoryService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]

        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAll(string? gardenName = null, string? plantName = null, string? seasonName = null)
        {
            try
            {
                List<GetPlantCropHistory> plantCropHistories = await _plantCropHistoryService.GetAllAsync(gardenName, plantName, seasonName);

                // Sắp xếp danh sách plantCropHistories theo ngày giảm dần
                plantCropHistories = plantCropHistories.OrderByDescending(cf => cf.CreatedDate).ToList();

                return Ok(new
                {
                    Data = plantCropHistories
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
        public async Task<IActionResult> CreatePlantCropHistory([FromBody] CreatePlantCropHistory createPlantCropHistory)
        {
            try
            {
                await _plantCropHistoryService.CreatePlantCropHistoryAsync(createPlantCropHistory);

                return Ok(new
                {
                    Message = "Plant crop history created successfully"
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



    }
}
