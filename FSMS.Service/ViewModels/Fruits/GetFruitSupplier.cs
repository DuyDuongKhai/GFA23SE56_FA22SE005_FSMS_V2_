using FSMS.Service.ViewModels.FruitImages;

namespace FSMS.Service.ViewModels.Fruits
{
    public class GetFruitSupplier
    {
        public int FruitId { get; set; }
        public string FruitName { get; set; }
        public int UserId { get; set; }
        public string FruitDescription { get; set; }
        public decimal Price { get; set; }
        public double QuantityAvailable { get; set; }
        public double QuantityInTransit { get; set; }
        public string OriginCity { get; set; }
        public string OrderType { get; set; }
        public string CategoryFruitName { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<GetFruitImage> FruitImages { get; set; }
    }
}
