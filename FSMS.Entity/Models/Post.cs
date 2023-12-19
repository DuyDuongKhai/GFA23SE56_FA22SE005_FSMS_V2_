using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public string PostTitle { get; set; } = null!;
        public string PostContent { get; set; } = null!;
        public string? PostImage { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
