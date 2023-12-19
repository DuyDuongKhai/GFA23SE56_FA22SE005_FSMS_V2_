using FSMS.Service.Services.WeatherServices;
using FSMS.Service.Utility;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Weather;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/weathers")]
    [ApiController]
    public class WeathersController : ControllerBase
    {
        private IWeatherService _weatherService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public WeathersController(IWeatherService weatherService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _weatherService = weatherService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Expert", "Farmer")]

        public async Task<IActionResult> GetAllFruitHistories(string? location = null, DateTime? createdDate = null, int userId = 0)
        {
            try
            {
                List<GetWeather> weatherData = await _weatherService.GetAllAsync(location, createdDate, userId);
                return Ok(new
                {
                    Data = weatherData
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
        [Route("scrape-weather-and-schedule")]

        [PermissionAuthorize("Expert")]
        public async Task<IActionResult> ScrapeWeatherAndSchedule(int userId)
        {
            try
            {
                await _weatherService.DeleteWeatherAsync(userId);

                await _weatherService.ScrapeCaMauCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeSocTrangCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeHauGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeCanThoCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeKienGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeAnGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeDongThapCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeVinhLongCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeTraVinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeBenTreCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeTienGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeLongAnCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeBinhDuongCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeBinhPhuocCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeDongNaiCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeTayNinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeBaRiaVungTauCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                await _weatherService.ScrapeHoChiMinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(userId);
                return Ok("Data scraped and saved to the database successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }


        }


    }
}
