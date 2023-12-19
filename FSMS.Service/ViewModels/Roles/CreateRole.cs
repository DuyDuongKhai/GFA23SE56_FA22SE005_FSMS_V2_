using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Roles
{
    public class CreateRole
    {
        [Required(ErrorMessage = "RoleName is required.")]
        [MaxLength(200, ErrorMessage = "RoleName must be less than or equals 200 characters.")]
        public string RoleName { get; set; }

    }
}
