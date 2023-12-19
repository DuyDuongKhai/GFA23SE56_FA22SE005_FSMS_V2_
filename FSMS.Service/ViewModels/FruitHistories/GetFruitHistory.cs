namespace FSMS.Service.ViewModels.FruitHistories
{
    public class GetFruitHistory
    {
        public int HistoryId { get; set; }
        public string FruitName { get; set; }
        public decimal Price { get; set; }
        public string FullName { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
