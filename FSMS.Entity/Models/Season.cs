using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Season
    {
        public int SeasonId { get; set; }
        public string SeasonName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public int GardenId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Garden Garden { get; set; } = null!;
    }
}
