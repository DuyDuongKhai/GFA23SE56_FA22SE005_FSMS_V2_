using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int? ParentOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string? PhoneNumber { get; set; }
        public string Type { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
