using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.WeatherRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Weather;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace FSMS.Service.Services.WeatherServices
{
    public class WeatherService : IWeatherService
    {
        private IWeatherRepository _weatherRepository;
        private IMapper _mapper;
        public WeatherService(IWeatherRepository weatherRepository, IMapper mapper)
        {
            _weatherRepository = weatherRepository;
            _mapper = mapper;
        }


        public async Task<List<GetWeather>> GetAllAsync(string? location = null, DateTime? createdDate = null, int userId = 0)
        {
            try
            {
                IEnumerable<FSMS.Entity.Models.Weather> weathers = await _weatherRepository.GetAsync(includeProperties: "User");

                // Filter data based on location if specified
                if (!string.IsNullOrWhiteSpace(location))
                {
                    weathers = weathers.Where(weather => weather.Location == location);
                }

                if (createdDate.HasValue)
                {
                    weathers = weathers.Where(weather =>
                        weather.CreatedDate != null &&
                        weather.CreatedDate.Value.Date == createdDate.Value.Date);
                }

                if (userId != 0)
                {
                    weathers = weathers.Where(weather => weather.UserId == userId);
                }

                var result = weathers
                    .Select(weather => _mapper.Map<GetWeather>(weather))
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching weather data.", ex);
            }
        }


        public async Task DeleteWeatherAsync(int userId)
        {
            try
            {
                // Xóa dữ liệu liên quan đến người dùng cụ thể
                var weathersToDelete = await _weatherRepository.GetAsync(weather => weather.UserId == userId);
                foreach (var weather in weathersToDelete)
                {
                    await _weatherRepository.DeleteAsync(weather);
                }

                await _weatherRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting fruit histories.", ex);
            }
        }


        public async Task ScrapeCaMauCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/ca-mau/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Cà Mau trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Cà Mau",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }

        public async Task ScrapeSocTrangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/soc-trang/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Sóc Trăng trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Sóc Trăng",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeHauGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/hau-giang/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Hậu Giang trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Hậu Giang",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeCanThoCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/can-tho/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Cần Thơ trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Cần Thơ",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeKienGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/kien-giang/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Kiên Giang trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Kiên Giang",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeAnGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/an-giang/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {

                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết An Giang trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "An Giang",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeDongThapCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/dong-thap/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Đồng Tháp trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Đồng Tháp",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeVinhLongCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/vinh-long/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Vĩnh Long trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Vĩnh Long",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeTraVinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/tra-vinh/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {

                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Trà Vinh trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Trà Vinh",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeBenTreCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/ben-tre/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Bến Tre trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Bến Tre",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeTienGiangCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/tien-giang/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Tiền Giang trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Tiền Giang",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeLongAnCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/long-an/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Long An trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Long An",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeBinhPhuocCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/binh-phuoc/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Bình Phước trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Bình Phước",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeBinhDuongCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/binh-duong/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Bình Dương trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Bình Dương",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeDongNaiCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/dong-nai/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Đồng Nai trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Đồng Nai",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeTayNinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/tay-ninh/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Tây Ninh trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Tây Ninh",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeBaRiaVungTauCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/ba-ria-vung-tau/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Vũng Tàu trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Vũng Tàu",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task ScrapeHoChiMinhCityWeatherForecast10DaysAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thoitiet.edu.vn/ho-chi-minh/10-ngay-toi";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Select the list of details elements
                    var detailElements = doc.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/div[1]/div[1]/div/div/details");

                    if (detailElements != null)
                    {
                        foreach (var detailElement in detailElements)
                        {
                            // Extract description and image data for each detail
                            var imageNode = detailElement.SelectSingleNode(".//div[2]/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            // Clean extra spaces in description
                            string description = CleanDescription(detailElement.InnerText.Trim());

                            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(imageSrc))
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = "Thời tiết Hồ Chí Minh trong 10 ngày tới",
                                    UserId = userId,
                                    Location = "Hồ Chí Minh",
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }

                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }

        private string CleanDescription(string description)
        {
            if (description != null)
            {
                // Remove all extra spaces, newlines, and tabs
                description = Regex.Replace(description, @"\s+", " ").Trim();
            }
            return description;
        }












    }
}
