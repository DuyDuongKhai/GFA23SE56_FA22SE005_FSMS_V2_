using FSMS.Service.ViewModels.Files;

namespace FSMS.Service.ViewModels.Users
{
    public class CreateUser : FileViewModel
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        /*public string ProfileImageUrl { get; set; }*/

        /*public string ImageMomoUrl { get; set; }*/
        public int RoleId { get; set; }

    }
}
