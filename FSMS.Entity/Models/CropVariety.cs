using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class CropVariety
    {
        public CropVariety()
        {
            CropVarietyStages = new HashSet<CropVarietyStage>();
            Plants = new HashSet<Plant>();
        }

        public int CropVarietyId { get; set; }
        public string CropVarietyName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Image { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<CropVarietyStage> CropVarietyStages { get; set; }
        public virtual ICollection<Plant> Plants { get; set; }
    }
}
