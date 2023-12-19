using FSMS.Service.Services.ReviewFruitServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.ReviewFruits;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/review-fruits")]
    [ApiController]
    public class ReviewFruitsController : ControllerBase
    {
        private IReviewFruitService _reviewFruitService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public ReviewFruitsController(IReviewFruitService reviewFruitService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _reviewFruitService = reviewFruitService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        //[PermissionAuthorize("Supplier", "Customer", "Farmer")]
        public async Task<IActionResult> GetAllReviewFruits(bool activeOnly = false, int? fruitId = null)
        {
            try
            {
                List<GetReviewFruit> reviewFruits = await _reviewFruitService.GetAllReviewFruitsAsync(activeOnly, fruitId);
                return Ok(new
                {
                    Data = reviewFruits
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

        [PermissionAuthorize("Supplier", "Customer", "Farmer")]
        public async Task<IActionResult> GetReviewFruitById(int id)
        {
            try
            {
                GetReviewFruit reviewFruit = await _reviewFruitService.GetAsync(id);
                return Ok(new
                {
                    Data = reviewFruit
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

        /*[PermissionAuthorize("Supplier", "Customer", "Farmer")]*/
        public async Task<IActionResult> CreateReviewFruit([FromForm] CreateReviewFruit createReviewFruit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _reviewFruitService.CreateReviewFruitAsync(createReviewFruit);

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

        [PermissionAuthorize("Supplier", "Customer", "Farmer")]
        public async Task<IActionResult> UpdateReviewProduct(int id, [FromForm] UpdateReviewFruit updateReviewFruit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _reviewFruitService.UpdateReviewFruitAsync(id, updateReviewFruit);
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

        [PermissionAuthorize("Supplier", "Customer", "Farmer")]
        public async Task<IActionResult> DeleteReviewFruitById(int id)
        {
            try
            {
                await _reviewFruitService.DeleteReviewFruitAsync(id);
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
