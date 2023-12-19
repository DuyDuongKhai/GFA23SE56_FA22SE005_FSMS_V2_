using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class CategoryFruit
    {
        public CategoryFruit()
        {
            Fruits = new HashSet<Fruit>();
        }

        public int CategoryFruitId { get; set; }
        public string CategoryFruitName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Fruit> Fruits { get; set; }
    }
}
