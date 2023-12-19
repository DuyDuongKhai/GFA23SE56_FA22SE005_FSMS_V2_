using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Notifications
{
    public class UpdateNotification
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string NotificationType { get; set; }

        [RegularExpression("^(Active|InActive)$", ErrorMessage = "Status must be 'Active' or 'InActive'.")]
        public DateTime? UpdateDate { get; set; }
        public string Status { get; set; }
    }
}
