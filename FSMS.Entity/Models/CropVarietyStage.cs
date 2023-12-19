using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class CropVarietyStage
    {
        public CropVarietyStage()
        {
            CropVarietyGrowthTasks = new HashSet<CropVarietyGrowthTask>();
        }

        public int CropVarietyStageId { get; set; }
        public int CropVarietyId { get; set; }
        public string StageName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual CropVariety CropVariety { get; set; } = null!;
        public virtual ICollection<CropVarietyGrowthTask> CropVarietyGrowthTasks { get; set; }
    }
}
