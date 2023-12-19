using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Files
{
    public class FilesViewModel
    {
        public List<IFormFile>? UploadFiles { get; set; }
    }
}
