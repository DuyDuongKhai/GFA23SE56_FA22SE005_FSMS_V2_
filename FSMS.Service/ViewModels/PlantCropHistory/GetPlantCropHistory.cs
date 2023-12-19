namespace FSMS.Service.ViewModels.PlantCropHistory
{
    public class GetPlantCropHistory
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public int GardenId { get; set; }
        public string GardenName { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
