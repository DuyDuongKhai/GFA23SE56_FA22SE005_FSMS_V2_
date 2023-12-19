namespace FSMS.Service.ViewModels.OrderDetails
{
    public class GetOrderDetail
    {
        public int FruitId { get; set; }
        public int FruitDiscountId { get; set; }
        public int UserId { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string FruitName { get; set; }
        public string FullName { get; set; }
        public string OderDetailType { get; set; }
        public string DiscountName { get; set; }
        public string Status { get; set; }
    }
}
