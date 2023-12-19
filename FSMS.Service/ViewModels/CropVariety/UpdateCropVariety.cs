using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.CropVariety
{
    public class UpdateCropVariety : FileViewModel
    {
        [Required(ErrorMessage = "Variety Name is required.")]
        public string CropVarietyName { get; set; }

        public string Description { get; set; }
        /*public string Image { get; set; }*/

        public string Status { get; set; }

    }
}
