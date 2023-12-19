namespace FSMS.Service.ViewModels.Weather
{
    public class GetWeather
    {
        public int WeatherId { get; set; }
        public string WeatherName { get; set; }
        public string FullName { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
