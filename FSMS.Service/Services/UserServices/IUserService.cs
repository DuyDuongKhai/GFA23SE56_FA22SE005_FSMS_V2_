using FSMS.Service.ViewModels.Users;

namespace FSMS.Service.Services.UserServices
{
    public interface IUserService
    {
        Task<List<GetUser>> GetAllAsync(string? fullName = null, bool activeOnly = false, string? roleName = null);
        Task<List<GetUserResponse>> GetAllUserAsync(string? fullName = null, bool activeOnly = false, string? roleName = null);
        Task<GetUser> GetAsync(int key);
        Task CreateUserAsync(CreateUser createUser);
        Task UpdateUserAsync(int key, UpdateUser updateUser);
        Task DeleteUserAsync(int key);
    }
}
