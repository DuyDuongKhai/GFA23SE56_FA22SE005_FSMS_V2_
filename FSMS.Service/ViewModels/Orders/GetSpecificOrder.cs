using FSMS.Service.ViewModels.OrderDetails;
using FSMS.Service.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Orders
{
    public class GetSpecificOrder
    {
        public int OrderId { get; set; }
        public string? ParentOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public List<GetOrderDetail> OrderDetails { get; set; }

    }
}
