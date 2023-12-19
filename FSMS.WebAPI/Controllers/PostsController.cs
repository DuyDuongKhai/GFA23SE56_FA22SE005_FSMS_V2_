using FSMS.Service.Services.PostServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations.Post;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Posts;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public PostsController(IPostService postService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _postService = postService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Expert", "Farmer", "Admin")]
        public async Task<IActionResult> GetAllPosts(string? postTitle = null, bool activeOnly = false, int? userId = null, string? type = null)
        {
            try
            {
                List<GetPost> posts = await _postService.GetAllAsync(postTitle, activeOnly, userId, type);
                posts = posts.OrderByDescending(c => c.CreatedDate).ToList();

                return Ok(new
                {
                    Data = posts
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

        [PermissionAuthorize("Expert", "Farmer", "Admin")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                GetPost post = await _postService.GetAsync(id);
                return Ok(new
                {
                    Data = post
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
        [Cache(1000)]

        [PermissionAuthorize("Expert", "Admin")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePost createPost)
        {
            var validator = new PostValidator();
            var validationResult = validator.Validate(createPost);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _postService.CreatePostAsync(createPost);

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
        [Cache(1000)]

        [PermissionAuthorize("Expert", "Admin")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePost updatePost)
        {
            var validator = new UpdatePostValidator();
            var validationResult = validator.Validate(updatePost);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _postService.UpdatePostAsync(id, updatePost);
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
        [Cache(1000)]

        [PermissionAuthorize("Expert", "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
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
        [HttpPut("{id}/process")]
        [Cache(1000)]

        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> ProcessPost(int id, [FromBody] ProcessPostRequest processPostRequest)
        {
            try
            {
                await _postService.ProcessPostAsync(id, processPostRequest);

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

        [HttpGet("expert-post-count")]
        [Cache(1000)]
        [PermissionAuthorize("Expert", "Admin", "Farmer")]
        public async Task<IActionResult> GetTop10ExpertPostCounts()
        {
            try
            {
                // Call the modified GetTop10ExpertPostCountsAsync method to get top 10 expert post counts
                IEnumerable<UserPostCount> top10ExpertPostCounts = await _postService.GetTop10ExpertPostCountsAsync();

                return Ok(top10ExpertPostCounts);
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
