using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.CropVarietyStages
{
    public class GetCropVarietyStage
    {
        public int CropVarietyStageId { get; set; }
        public string CropVarietyName { get; set; }
        public string StageName { get; set; } 
        public string Description { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
