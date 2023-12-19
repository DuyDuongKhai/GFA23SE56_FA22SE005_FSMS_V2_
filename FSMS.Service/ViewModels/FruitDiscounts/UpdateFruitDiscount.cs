namespace FSMS.Service.ViewModels.FruitDiscounts
{
    public class UpdateFruitDiscount
    {
        public int FruitId { get; set; }
        public string DiscountName { get; set; }
        public int DiscountThreshold { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime DiscountExpiryDate { get; set; }
        public string Status { get; set; }

    }
}
