using FSMS.Service.ViewModels.Comments;

namespace FSMS.Service.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<GetComment>> GetAllAsync(bool activeOnly = false, int postId = 0);
        Task<GetComment> GetAsync(int key);
        Task CreateCommentAsync(CreateComment createComment);
        Task UpdateCommentAsync(int key, UpdateComment updateComment);
        Task DeleteCommentAsync(int key);
    }

}
