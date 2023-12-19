using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Comments
{
    public class GetComment
    {
        public int CommentId { get; set; }
        public string PostContent { get; set; }
        public string FullName { get; set; }
        public string CommentContent { get; set; }
        public int ParentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Status { get; set; }
    }
}
