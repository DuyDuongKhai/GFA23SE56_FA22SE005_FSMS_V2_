using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Notifications
{
    public class CreateNotification
    {
        public string Message { get; set; }
        public int UserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string NotificationType { get; set; }
    }
}
