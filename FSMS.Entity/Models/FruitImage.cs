using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class FruitImage
    {
        public int FruitImageId { get; set; }
        public int FruitId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Status { get; set; }

        public virtual Fruit Fruit { get; set; } = null!;
    }
}
