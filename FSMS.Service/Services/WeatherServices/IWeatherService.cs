using FSMS.Service.ViewModels.Weather;

namespace FSMS.Service.Services.WeatherServices
{
    public interface IWeatherService
    {
        Task<List<GetWeather>> GetAllAsync(string? location = null, DateTime? createdDate = null, int userId = 0);

        Task ScrapeCaMauCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeSocTrangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeHauGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeCanThoCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeKienGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeAnGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeDongThapCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeVinhLongCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeTraVinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeBenTreCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeTienGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeLongAnCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeBinhPhuocCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeBinhDuongCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeDongNaiCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeTayNinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeBaRiaVungTauCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);
        Task ScrapeHoChiMinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId);

        Task DeleteWeatherAsync(int userId);

    }
}
