using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class CropVarietyGrowthTask
    {
        public int GrowthTaskId { get; set; }
        public int CropVarietyStageId { get; set; }
        public string TaskName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual CropVarietyStage CropVarietyStage { get; set; } = null!;
    }
}
