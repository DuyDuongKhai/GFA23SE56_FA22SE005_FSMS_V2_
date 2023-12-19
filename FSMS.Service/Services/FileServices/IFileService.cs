using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.FileServices
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file);
        public Task<List<string>> UploadFiles(List<IFormFile> files);
        public Task DeleteProductFile(string[] urlImages);
        /*public Task DeleteAvatar(Guid id);*/
        /*public Task DeleteLogo(Guid id);*/
    }
}
