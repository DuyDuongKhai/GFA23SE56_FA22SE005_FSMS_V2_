using Microsoft.AspNetCore.Http;

namespace FSMS.Service.ViewModels.Files
{
    public class FileViewModel
    {
        public IFormFile? UploadFile { get; set; }
    }
}
