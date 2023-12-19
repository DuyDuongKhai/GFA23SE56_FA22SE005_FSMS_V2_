using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.CategoryFruits
{
    public class GetCategoryFruit
    {
        public int CategoryFruitId { get; set; }
        public string CategoryFruitName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Status { get; set; }
    }
}
