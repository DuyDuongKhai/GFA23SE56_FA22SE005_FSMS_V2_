using FSMS.Service.ViewModels.OrderDetails;

namespace FSMS.Service.ViewModels.Orders
{
    public class GetOrder
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<GetOrderDetail> OrderDetails { get; set; }


    }
}
