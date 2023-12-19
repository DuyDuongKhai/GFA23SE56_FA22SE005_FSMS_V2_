
using FSMS.Service.ViewModels.Files;
using System.ComponentModel.DataAnnotations;

namespace FSMS.Service.ViewModels.FruitImages
{
    public class UpdateFruitImage : FileViewModel
    {
        [RegularExpression("^(Active|InActive)$", ErrorMessage = "Status must be 'Active' or 'InActive'.")]
        public string Status { get; set; }
    }
}
