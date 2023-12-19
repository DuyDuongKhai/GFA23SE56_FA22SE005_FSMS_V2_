using FSMS.Entity.Models;
using FSMS.Service.ViewModels.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Payments
{
    public class PaymentWithOrderDetails
    {
        public Payment Payment { get; set; }
        public List<GetOrderDetail> OrderDetails { get; set; }
    }
}
