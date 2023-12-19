using FSMS.Service.Utility.ValidationAttributes;
using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.GardenTasks
{
    public class UpdateGardenTask : FileViewModel
    {
        public string GardenTaskName { get; set; }

        public string Description { get; set; }

        public DateTime GardenTaskDate { get; set; }
        /*public string Image { get; set; }*/
        public string Status { get; set; }
    }
}
