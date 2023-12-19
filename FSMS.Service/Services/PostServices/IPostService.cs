using FSMS.Service.ViewModels.Posts;

namespace FSMS.Service.Services.PostServices
{
    public interface IPostService
    {
        Task<List<GetPost>> GetAllAsync(string? postTitle = null, bool activeOnly = false, int? userId = null, string? type = null);
        Task<GetPost> GetAsync(int key);
        Task CreatePostAsync(CreatePost createPost);
        Task UpdatePostAsync(int key, UpdatePost updatePost);
        Task DeletePostAsync(int key);

        Task ProcessPostAsync(int postId, ProcessPostRequest processPostRequest);
        Task<IEnumerable<UserPostCount>> GetTop10ExpertPostCountsAsync();

    }
}
