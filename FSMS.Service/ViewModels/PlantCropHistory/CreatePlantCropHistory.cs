namespace FSMS.Service.ViewModels.PlantCropHistory
{
    public class CreatePlantCropHistory
    {
        public int PlantId { get; set; }
        public int SeasonId { get; set; }
        public int GardenId { get; set; }
        public double Quantity { get; set; }
    }
}