using FSMS.Service.Services.FruitImageServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.FruitImages;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/fruit-images")]
    [ApiController]
    public class FruitImagesController : ControllerBase
    {
        private IFruitImageService _fruitImageService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public FruitImagesController(IFruitImageService fruitImageService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _fruitImageService = fruitImageService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> GetAllFruitImages()
        {
            try
            {
                List<GetFruitImage> fruitImages = await _fruitImageService.GetAllAsync();
                return Ok(new
                {
                    Data = fruitImages
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Cache(1000)]

        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> GetFruitImageById(int id)
        {
            try
            {
                GetFruitImage fruitImage = await _fruitImageService.GetAsync(id);
                return Ok(new
                {
                    Data = fruitImage
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPost]

        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> CreateFruitImage([FromForm] CreateFruitImage createFruitImage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _fruitImageService.CreateFruitImageAsync(createFruitImage);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{id}")]

        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> UpdateFruitImage(int id, [FromForm] UpdateFruitImage updateFruitImage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _fruitImageService.UpdateFruitImageAsync(id, updateFruitImage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]

        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> DeleteFruitImage(int id)
        {
            try
            {
                await _fruitImageService.DeleteFruitImageAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
