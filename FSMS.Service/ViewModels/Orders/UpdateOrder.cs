using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Orders
{
    public class UpdateOrder
    {

        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;

        [Required(ErrorMessage = "PhoneNumber is required.")]
        public string PhoneNumber { get; set; }

        public string Status { get; set; }
    }
}
