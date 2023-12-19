using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Garden
    {
        public Garden()
        {
            GardenTasks = new HashSet<GardenTask>();
            Plants = new HashSet<Plant>();
            Seasons = new HashSet<Season>();
        }

        public int GardenId { get; set; }
        public string GardenName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int UserId { get; set; }
        public string? Region { get; set; }
        public string? Image { get; set; }
        public double? QuantityPlanted { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<GardenTask> GardenTasks { get; set; }
        public virtual ICollection<Plant> Plants { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
    }
}
