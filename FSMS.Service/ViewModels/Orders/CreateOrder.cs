using FSMS.Service.ViewModels.OrderDetails;

namespace FSMS.Service.ViewModels.Orders
{
    public class CreateOrder
    {
        public int UserId { get; set; }
        public string DeliveryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public List<CreateOrderDetail> OrderDetails { get; set; }

    }
}
