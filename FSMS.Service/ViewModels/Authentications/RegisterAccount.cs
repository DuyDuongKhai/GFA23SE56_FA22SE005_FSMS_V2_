using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FSMS.Service.ViewModels.Authentications
{
    public class RegisterAccount
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(200, ErrorMessage = "Full Name must be less than or equals 200 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(200, ErrorMessage = "Password must be less than or equals 200 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [Phone(ErrorMessage = "PhoneNumber email format.")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        /*  [Url(ErrorMessage = "Invalid URL format for Profile Image.")]
          public string ImageMomoUrl { get; set; }*/


        /*  [Url(ErrorMessage = "Invalid URL format for Profile Image.")]
          public string ProfileImageUrl { get; set; }*/

        public IFormFile? ProfileImageUrl { get; set; } = null;

        [Required(ErrorMessage = "RoleID is required.")]
        public int RoleId { get; set; }
    }
}
