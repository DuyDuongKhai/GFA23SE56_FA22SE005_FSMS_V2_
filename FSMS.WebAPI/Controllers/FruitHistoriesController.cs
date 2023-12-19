using FSMS.Service.Services.FruitHistoryServices;
using FSMS.Service.Utility;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.FruitHistories;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/fruit-histories")]
    [ApiController]
    public class FruitHistoriesController : ControllerBase
    {
        private IFruitHistoryService _fruitHistoryService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public FruitHistoriesController(IFruitHistoryService fruitHistoryService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _fruitHistoryService = fruitHistoryService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Expert", "Farmer")]
        public async Task<IActionResult> GetAllFruitHistories(string? location = null, DateTime? createdDate = null, int userId = 0)
        {
            try
            {
                List<GetFruitHistory> fruitHistories = await _fruitHistoryService.GetAllAsync(location, createdDate, userId);
                return Ok(new
                {
                    Data = fruitHistories
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

        [Route("scrape-fruit-histories")]
        [PermissionAuthorize("Expert")]
        public async Task<IActionResult> ScheduleFruitHistories(int userId)
        {
            try
            {
                await _fruitHistoryService.DeleteFruitHistoryAsync(userId);
                await _fruitHistoryService.ScrapeFarmerMarketAndSaveToDatabaseAsync(userId);
                await _fruitHistoryService.ScrapeThucPhamNhanhAndSaveToDatabaseAsync(userId);
                return Ok("Data scraped and saved to the database successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}
