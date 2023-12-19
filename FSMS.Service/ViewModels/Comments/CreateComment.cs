using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Comments
{
    public class CreateComment
    {
        [Required(ErrorMessage = "Comment Content is required.")]
        [MaxLength(200, ErrorMessage = "Comment Content  must be less than or equals 200 characters.")]
        public string CommentContent { get; set; }

        [Required(ErrorMessage = "PostID is required.")]
        public int PostId { get; set; }
        public int ParentId { get; set; }

        [Required(ErrorMessage = "UserID is required.")]
        public int UserId { get; set; }

    }
}
