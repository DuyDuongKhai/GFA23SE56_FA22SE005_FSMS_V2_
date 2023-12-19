using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.GardenTasks
{
    public class CreateGardenTask :FileViewModel
    {
      
        public string GardenTaskName { get; set; }

       
        public string Description { get; set; }

        public DateTime GardenTaskDate { get; set; }

        public int GardenId { get; set; }
        public int PlantId { get; set; }

        /*public string Image { get; set; }*/

    }
 
}
