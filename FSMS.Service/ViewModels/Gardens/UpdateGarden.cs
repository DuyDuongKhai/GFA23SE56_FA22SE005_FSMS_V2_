using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Gardens
{
    public class UpdateGarden : FileViewModel
    {
        public string GardenName { get; set; }
        public string Description { get; set; }
        /*public string Image { get; set; }*/

        public string Region { get; set; }

        public string Status { get; set; }

        public double QuantityPlanted { get; set; }

    }
}
