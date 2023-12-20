namespace FSMS.Service.ViewModels.PlantCropHistory
{
    public class PlantHarvestStats
    {

        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public double Quantity { get; set; }
    }
}
