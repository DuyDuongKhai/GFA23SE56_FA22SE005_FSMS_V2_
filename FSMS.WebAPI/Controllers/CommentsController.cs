using FSMS.Service.Services.CommentServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Comments;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService _commentService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public CommentsController(ICommentService commentService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _commentService = commentService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        [PermissionAuthorize("Farmer", "Expert", "Admin")]
        public async Task<IActionResult> GetAllComments(bool activeOnly = false, int postId = 0)
        {
            try
            {
                List<GetComment> comments = await _commentService.GetAllAsync(activeOnly, postId);

                return Ok(new
                {
                    Data = comments
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
        [PermissionAuthorize("Farmer", "Expert", "Admin")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            try
            {
                GetComment comment = await _commentService.GetAsync(id);
                return Ok(new
                {
                    Data = comment
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
        [PermissionAuthorize("Farmer", "Expert", "Admin")]
        public async Task<IActionResult> CreateComment([FromBody] CreateComment createComment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _commentService.CreateCommentAsync(createComment);

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
        [PermissionAuthorize("Farmer", "Expert", "Admin")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateComment updateComment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _commentService.UpdateCommentAsync(id, updateComment);
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
        [PermissionAuthorize("Farmer", "Expert", "Admin")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
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
