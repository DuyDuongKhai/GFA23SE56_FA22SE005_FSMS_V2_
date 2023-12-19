using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class PlantCropHistory
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int SeasonId { get; set; }
        public int GardenId { get; set; }
        public double Quantity { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Plant Plant { get; set; } = null!;
    }
}
