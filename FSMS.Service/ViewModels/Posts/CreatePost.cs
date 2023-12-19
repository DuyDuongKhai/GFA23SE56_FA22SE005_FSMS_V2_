using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Posts
{
    public class CreatePost : FileViewModel
    {       
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        /*public string PostImage { get; set; }*/
        public string Type { get; set; }
        public int UserId { get; set; }
    }
}
