using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string NotificationType { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
