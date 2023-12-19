namespace FSMS.Service.ViewModels.Gardens
{
    public class GetGarden
    {
        public int GardenId { get; set; }
        public string GardenName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string FullName { get; set; }
        public double QuantityPlanted { get; set; }
        public double TotalHarvestQuantity { get; set; }
        public string Region { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
