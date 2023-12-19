using System.ComponentModel.DataAnnotations;

namespace FSMS.Service.ViewModels.Authentications
{
    public class ConfirmAccountRequest
    {


        [Required(ErrorMessage = "OTP is required")]
        public string OTP { get; set; }
    }

}
