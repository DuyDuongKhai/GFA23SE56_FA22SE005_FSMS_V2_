using FSMS.Service.Services.FileServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private static string ApiKey = "AIzaSyC6E0ovmbqlUYosOzwras-w5SP1bSrSfOU";
        private static string Bucket = "capstonep-30015.appspot.com";
        private static string AuthEmail = "minhvdse150355@fpt.edu.vn";
        private static string AuthPassword = "123456";
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }
       
        /// <summary>
        /// [Guest] Endpoint for upload image with condition
        /// </summary>
        /// <param name="files"></param>
        /// <returns>Link of image in firebase storage</returns>
        /// <response code="200">Returns the Link of image</response>
        /// <response code="204">Returns if link of image is empty</response>
        /// <response code="403">Return if token is access denied</response>
        [HttpPost]
        public async Task<ActionResult> PostList(List<IFormFile> files)
        {
            try
            {
                await _fileService.UploadFiles(files);
                return Ok("Upload file success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
       
    }
}

