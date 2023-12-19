using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Plant
    {
        public Plant()
        {
            Fruits = new HashSet<Fruit>();
            GardenTasks = new HashSet<GardenTask>();
            PlantCropHistories = new HashSet<PlantCropHistory>();
        }

        public int PlantId { get; set; }
        public string PlantName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime PlantingDate { get; set; }
        public DateTime? HarvestingDate { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public int GardenId { get; set; }
        public int CropVarietyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? QuantityPlanted { get; set; }
        public double? EstimatedHarvestQuantity { get; set; }

        public virtual CropVariety CropVariety { get; set; } = null!;
        public virtual Garden Garden { get; set; } = null!;
        public virtual ICollection<Fruit> Fruits { get; set; }
        public virtual ICollection<GardenTask> GardenTasks { get; set; }
        public virtual ICollection<PlantCropHistory> PlantCropHistories { get; set; }
    }
}
