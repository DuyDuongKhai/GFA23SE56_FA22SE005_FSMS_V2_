using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class FruitDiscount
    {
        public FruitDiscount()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int FruitDiscountId { get; set; }
        public int FruitId { get; set; }
        public string? DiscountName { get; set; }
        public int? DiscountThreshold { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? DiscountExpiryDate { get; set; }
        public decimal? DepositAmount { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Fruit Fruit { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
