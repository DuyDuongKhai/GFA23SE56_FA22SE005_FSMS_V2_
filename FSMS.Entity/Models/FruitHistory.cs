using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class FruitHistory
    {
        public int HistoryId { get; set; }
        public string FruitName { get; set; } = null!;
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
