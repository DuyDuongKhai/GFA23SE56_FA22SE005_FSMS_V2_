using FSMS.Service.ViewModels.Files;
using System.ComponentModel.DataAnnotations;

namespace FSMS.Service.ViewModels.ReviewFruits
{
    public class CreateReviewFruit : FileViewModel
    {
        [Required(ErrorMessage = "Review Comment is required.")]
        [MaxLength(200, ErrorMessage = "ReviewComment  must be less than or equals 200 characters.")]
        public string ReviewComment { get; set; }

        public decimal Rating { get; set; }

        /*public string ReviewImageUrl { get; set; }*/

        [Required(ErrorMessage = "Fruit ID is required.")]
        public int FruitId { get; set; }
        public int ParentId { get; set; }


        [Required(ErrorMessage = "UserID is required.")]
        public int UserId { get; set; }
    }
}
