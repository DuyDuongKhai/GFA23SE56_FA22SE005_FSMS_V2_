using FSMS.Service.ViewModels.Files;
using System.ComponentModel.DataAnnotations;

namespace FSMS.Service.ViewModels.ReviewFruits
{
    public class UpdateReviewFruit : FileViewModel
    {
        [Required(ErrorMessage = "Review Comment is required.")]
        [MaxLength(200, ErrorMessage = "ReviewComment  must be less than or equals 200 characters.")]
        public string ReviewComment { get; set; }

        public decimal Rating { get; set; }

        /* public string ReviewImageUrl { get; set; }*/

        [RegularExpression("^(Active|InActive)$", ErrorMessage = "Status must be 'Active' or 'InActive'.")]
        public string Status { get; set; }
    }
}
