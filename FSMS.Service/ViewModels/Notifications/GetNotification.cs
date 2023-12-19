using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Notifications
{
    public class GetNotification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } 
        public bool IsRead { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string NotificationType { get; set; }
    }
}
