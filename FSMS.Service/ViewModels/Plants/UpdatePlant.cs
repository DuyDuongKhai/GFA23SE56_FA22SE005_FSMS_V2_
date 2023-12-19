using FSMS.Service.ViewModels.Files;

namespace FSMS.Service.ViewModels.Plants
{
    public class UpdatePlant : FileViewModel
    {
        public string PlantName { get; set; }
        public string Description { get; set; }

        public DateTime PlantingDate { get; set; }

        public DateTime HarvestingDate { get; set; }

        public double QuantityPlanted { get; set; }


        public double EstimatedHarvestQuantity { get; set; }

        public string Status { get; set; }

    }
}
