using FSMS.Service.ViewModels.Files;

namespace FSMS.Service.ViewModels.Plants
{
    public class CreatePlant : FileViewModel
    {


        public string PlantName { get; set; }
        public string Description { get; set; }
        /*public string Image { get; set; }*/
        public DateTime PlantingDate { get; set; }
        public DateTime HarvestingDate { get; set; }
        public int GardenId { get; set; }
        public int CropVarietyId { get; set; }
        public string Status { get; set; }
        public double QuantityPlanted { get; set; }

    }
}
