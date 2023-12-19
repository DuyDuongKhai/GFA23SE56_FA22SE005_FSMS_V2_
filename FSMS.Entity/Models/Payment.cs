using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Status { get; set; }
        public int UserId { get; set; }

        public virtual Order? Order { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
