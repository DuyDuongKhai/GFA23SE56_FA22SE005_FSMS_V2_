using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Weather
{
    public class CreateWeather
    {
        public string WeatherName { get; set; }
        public int UserId { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}
