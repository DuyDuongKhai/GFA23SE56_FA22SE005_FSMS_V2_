using FSMS.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Entity.Repositories.WeatherRepositories
{
    public class WeatherRepository : RepositoryBase<Weather>, IWeatherRepository
    {
        public WeatherRepository() { }
    }
}
