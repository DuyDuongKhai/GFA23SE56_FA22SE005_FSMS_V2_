using FSMS.Service.ViewModels.Plants;
using System.Text.Json.Serialization;

namespace FSMS.Service.ViewModels.CropVariety
{
    public class GetCropVariety
    {
        public int CropVarietyId { get; set; }
        public string CropVarietyName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [JsonIgnore]
        public List<GetPlant> Plants { get; set; }


    }
}
