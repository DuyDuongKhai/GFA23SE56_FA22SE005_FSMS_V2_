using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.FruitImages
{
    public class GetFruitImage
    {
        public int FruitImageId { get; set; }
        public string FruitName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Status { get; set; }
    }
}
