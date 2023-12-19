using FSMS.Service.Services.CategoryFruitServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.CategoryFruits;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/category-products")]

    [ApiController]
    public class CategoryFruitsController : ControllerBase
    {
        private ICategoryFruitService _categoryFruitService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public CategoryFruitsController(ICategoryFruitService categoryFruitService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _categoryFruitService = categoryFruitService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        [HttpGet]
        [Cache(1000)]

        //[PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> GetAll(string? categoryName = null, bool activeOnly = false)
        {
            try
            {
                List<GetCategoryFruit> categoryFruits = await _categoryFruitService.GetAllAsync(categoryName, activeOnly);

                // Sắp xếp danh sách categoryFruits theo ngày giảm dần
                categoryFruits = categoryFruits.OrderByDescending(cf => cf.CreatedDate).ToList();

                return Ok(new
                {
                    Data = categoryFruits
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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                GetCategoryFruit categoryFruit = await _categoryFruitService.GetAsync(id);
                return Ok(new
                {
                    Data = categoryFruit
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
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryFruit createCategoryFruit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _categoryFruitService.CreateCategoryFruitAsync(createCategoryFruit);

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
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryFruit updateCategoryFruit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _categoryFruitService.UpdateCategoryFruitAsync(id, updateCategoryFruit);
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
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryFruitService.DeleteCategoryFruitAsync(id);
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
