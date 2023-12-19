using FSMS.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Authentications
{
    public class SignInAccount
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
