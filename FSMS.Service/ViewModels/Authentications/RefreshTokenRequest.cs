using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Authentications
{
    public class RefreshTokenRequest
    {
        [Required]
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
