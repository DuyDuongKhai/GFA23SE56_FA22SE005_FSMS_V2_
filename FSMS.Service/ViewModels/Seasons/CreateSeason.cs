using FSMS.Service.Utility.ValidationAttributes;
using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Seasons
{
    public class CreateSeason : FileViewModel
    {
        public string SeasonName { get; set; }
        public string Description { get; set; }
        /*public string Image { get; set; }*/

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GardenId { get; set; }

    }
}
