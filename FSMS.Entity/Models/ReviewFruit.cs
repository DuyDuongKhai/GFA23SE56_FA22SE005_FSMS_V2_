using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class ReviewFruit
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int FruitId { get; set; }
        public decimal Rating { get; set; }
        public string ReviewComment { get; set; } = null!;
        public string? ReviewImageUrl { get; set; }
        public int? ParentId { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Fruit Fruit { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
