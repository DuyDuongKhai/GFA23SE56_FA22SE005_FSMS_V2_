using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.OrderDetails
{
    public class CreateOrderDetail
    {
       /* [Required(ErrorMessage = "Product id is required.")]*/
        public int FruitId { get; set; }
        public int FruitDiscountId { get; set; }


      /*  [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 100, ErrorMessage = "Quanlity of Product can not order less than 0 product or more than 100 products.")]*/
        public int Quantity { get; set; }

    }
}
