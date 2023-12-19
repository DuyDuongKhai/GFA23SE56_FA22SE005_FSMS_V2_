using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Fruit
    {
        public Fruit()
        {
            FruitDiscounts = new HashSet<FruitDiscount>();
            FruitImages = new HashSet<FruitImage>();
            OrderDetails = new HashSet<OrderDetail>();
            ReviewFruits = new HashSet<ReviewFruit>();
        }

        public int FruitId { get; set; }
        public string FruitName { get; set; } = null!;
        public string? FruitDescription { get; set; }
        public decimal Price { get; set; }
        public double QuantityAvailable { get; set; }
        public double? QuantityInTransit { get; set; }
        public string? OriginCity { get; set; }
        public string? OrderType { get; set; }
        public int UserId { get; set; }
        public int CategoryFruitId { get; set; }
        public int? PlantId { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual CategoryFruit CategoryFruit { get; set; } = null!;
        public virtual Plant? Plant { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<FruitDiscount> FruitDiscounts { get; set; }
        public virtual ICollection<FruitImage> FruitImages { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ReviewFruit> ReviewFruits { get; set; }
    }
}
