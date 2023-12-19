using FSMS.Service.Services.FruitDiscountSevices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations.FruitDiscount;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.FruitDiscounts;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/fruit-discounts")]
    [ApiController]
    public class FruitDiscountsController : ControllerBase
    {
        private IFruitDiscountService _fruitDiscountService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public FruitDiscountsController(IFruitDiscountService fruitDiscountService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _fruitDiscountService = fruitDiscountService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Supplier", "Farmer", "Customer")]
        public async Task<IActionResult> GetAllFruitDiscounts(string? discountName = null, DateTime? discountExpiryDate = null, bool activeOnly = false, int? userId = null, int? fruitId = null)
        {
            try
            {
                List<GetFruitDiscount> fruitDiscounts = await _fruitDiscountService.GetAllAsync(discountName, discountExpiryDate, activeOnly, userId, fruitId);

                fruitDiscounts = fruitDiscounts.OrderByDescending(c => c.CreatedDate).ToList();


                return Ok(new
                {
                    Data = fruitDiscounts
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

        [PermissionAuthorize("Supplier", "Farmer", "Customer")]
        public async Task<IActionResult> GetFruitDiscountById(int id)
        {
            try
            {
                GetFruitDiscount fruitDiscount = await _fruitDiscountService.GetAsync(id);
                return Ok(new
                {
                    Data = fruitDiscount
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
        public async Task<IActionResult> CreateFruitDiscount([FromBody] CreateFruitDiscount createFruitDiscount)
        {
            var validator = new FruitDiscountValidator();
            var validationResult = validator.Validate(createFruitDiscount);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _fruitDiscountService.CreateFruitDiscountAsync(createFruitDiscount);

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
        public async Task<IActionResult> UpdateFruitDiscount(int id, [FromBody] UpdateFruitDiscount updateFruitDiscount)
        {
            var validator = new UpdateFruitDiscountValidator();
            var validationResult = validator.Validate(updateFruitDiscount);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _fruitDiscountService.UpdateFruitDiscountAsync(id, updateFruitDiscount);
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
        public async Task<IActionResult> DeleteFruitDiscountById(int id)
        {
            try
            {
                await _fruitDiscountService.DeleteFruitDiscountAsync(id);
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
