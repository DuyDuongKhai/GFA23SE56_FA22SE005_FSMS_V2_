using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class GardenTask
    {
        public int GardenTaskId { get; set; }
        public string GardenTaskName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime GardenTaskDate { get; set; }
        public string? Status { get; set; }
        public int GardenId { get; set; }
        public int PlantId { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Garden Garden { get; set; } = null!;
        public virtual Plant Plant { get; set; } = null!;
    }
}
