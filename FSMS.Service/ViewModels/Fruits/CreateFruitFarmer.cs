using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Fruits
{
    public class CreateFruitFarmer
    {
        public string FruitName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public double QuantityAvailable { get; set; }
        public double QuantityInTransit { get; set; }
        public string OriginCity { get; set; }
        public string OrderType { get; set; }
        public int CategoryFruitId { get; set; }
        public int PlantId { get; set; }
        public int UserId { get; set; }
    }
}
