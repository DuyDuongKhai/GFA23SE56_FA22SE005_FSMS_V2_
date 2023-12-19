using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Roles
{
   public class GetRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } 
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
