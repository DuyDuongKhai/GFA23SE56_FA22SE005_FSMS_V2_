using FSMS.Service.Utility.ValidationAttributes;
using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.CropVariety
{
    public class CreateCropVariety : FileViewModel
    {

        [Required(ErrorMessage = "Variety Nameis required.")]
        [MaxLength(200, ErrorMessage = "Variety Name must be less than or equals 200 characters.")]
        public string CropVarietyName { get; set; }

        [MaxLength(255, ErrorMessage = "Description must be less than or equals 255 characters.")]
        public string Description { get; set; }

        /*public string Image { get; set; }*/

    }
}
