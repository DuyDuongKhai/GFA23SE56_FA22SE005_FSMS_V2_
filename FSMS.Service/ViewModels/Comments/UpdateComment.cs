using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Comments
{
    public class UpdateComment
    {
        [Required(ErrorMessage = "Comment Content is required.")]
        [MaxLength(200, ErrorMessage = "Comment Content  must be less than or equals 200 characters.")]
        public string CommentContent { get; set; }

        [RegularExpression("^(Active|InActive)$", ErrorMessage = "Status must be 'Active' or 'InActive'.")]
        public string Status { get; set; }

    }
}
