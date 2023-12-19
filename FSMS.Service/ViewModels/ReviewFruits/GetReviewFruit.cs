using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.ReviewFruits
{
    public class GetReviewFruit
    {
        public int ReviewId { get; set; }
        public string FullName { get; set; }
        public string FruitName { get; set; }
        public decimal Rating { get; set; }
        public string ReviewComment { get; set; }
        public string ReviewImageUrl { get; set; }
        public int ParentId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
