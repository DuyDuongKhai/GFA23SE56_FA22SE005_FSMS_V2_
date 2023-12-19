using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class ChatHistory
    {
        public int Id { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string Message { get; set; } = null!;
        public DateTime SendTimeOnUtc { get; set; }

        public virtual User ReceiverNavigation { get; set; } = null!;
        public virtual User SenderNavigation { get; set; } = null!;
    }
}
