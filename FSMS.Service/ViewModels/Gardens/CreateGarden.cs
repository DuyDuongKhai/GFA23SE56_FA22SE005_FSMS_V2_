using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Gardens
{
    public class CreateGarden : FileViewModel
    {
       /* [Required(ErrorMessage = "GardenName is required.")]
        [MaxLength(200, ErrorMessage = "GardenName must be less than or equals 200 characters.")]*/
        public string GardenName { get; set; }
       /* [MaxLength(255, ErrorMessage = "Supplier Description must be less than or equals 255 characters.")]*/
        public string Description { get; set; }
        /*public string Image { get; set; }*/

        /*  [MaxLength(255, ErrorMessage = "Region must be less than or equals 255 characters.")]*/
        public string Region { get; set; }

       /* [Required(ErrorMessage = "UserID is required.")]*/
        public int UserId { get; set; }

    }
}
