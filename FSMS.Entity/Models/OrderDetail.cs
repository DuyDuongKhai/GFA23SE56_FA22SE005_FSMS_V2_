using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int FruitId { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string? OderDetailType { get; set; }
        public int? FruitDiscountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Status { get; set; }

        public virtual Fruit Fruit { get; set; } = null!;
        public virtual FruitDiscount? FruitDiscount { get; set; }
        public virtual Order Order { get; set; } = null!;
    }
}
