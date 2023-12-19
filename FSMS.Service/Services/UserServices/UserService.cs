using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.RoleRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Users;

namespace FSMS.Service.Services.UserServices
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private readonly IFileService _fileService;

        private IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _fileService = fileService;
        }

        public async Task CreateUserAsync(CreateUser createUser)
        {
            try
            {
                // Kiểm tra xem có người dùng với cùng địa chỉ email và bất kỳ vai trò nào không.
                bool emailExists = (await _userRepository.GetAsync())
                    .Any(c => c.Email == createUser.Email);

                if (emailExists)
                {
                    throw new BadRequestException("A user with the same email address already exists.");
                }

                int lastId = (await _userRepository.GetAsync()).Max(x => x.UserId);
                User user = new User()
                {
                    FullName = createUser.FullName,
                    Password = createUser.Password,
                    Email = createUser.Email,
                    PhoneNumber = createUser.PhoneNumber,
                    Address = createUser.Address,
                    /*                    ProfileImageUrl = createUser.ProfileImageUrl,
                    *//*                    ImageMomoUrl = createUser.ImageMomoUrl,*/
                    ImageMomoUrl = "0",
                    RoleId = createUser.RoleId,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    UserId = lastId + 1
                };
                if (createUser.UploadFile == null)
                {
                    user.ProfileImageUrl = "";
                }
                else if (createUser.UploadFile != null) user.ProfileImageUrl = await _fileService.UploadFile(createUser.UploadFile);
                await _userRepository.InsertAsync(user);
                await _userRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task DeleteUserAsync(int key)
        {
            try
            {
                User existedUser = await _userRepository.GetByIDAsync(key);

                if (existedUser == null)
                {
                    throw new Exception("UserId does not exist in the system.");
                }
                if (existedUser.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("User is not active.");
                }

                existedUser.Status = StatusEnums.InActive.ToString();

                await _userRepository.UpdateAsync(existedUser);
                await _userRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task<GetUser> GetAsync(int key)
        {
            try
            {
                User user = await _userRepository.GetByIDAsync(key);

                if (user == null)
                {
                    throw new Exception("User ID does not exist in the system.");
                }
                if (user.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("User is not active.");
                }

                List<GetUser> users = _mapper.Map<List<GetUser>>(
                    await _userRepository.GetAsync(includeProperties: "Role")
                );

                GetUser result = _mapper.Map<GetUser>(user);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<GetUser>> GetAllAsync(string? fullName = null, bool activeOnly = false, string? roleName = null)
        {
            try
            {
                IEnumerable<User> users = await _userRepository.GetAsync(includeProperties: "Role");

                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    users = users.Where(user => user.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    users = users.Where(user => user.Role.RoleName.Contains(roleName, StringComparison.OrdinalIgnoreCase));
                }

                if (activeOnly)
                {
                    users = users.Where(user => user.Status == StatusEnums.Active.ToString());
                }

                List<GetUser> result = _mapper.Map<List<GetUser>>(users);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<GetUserResponse>> GetAllUserAsync(string? fullName = null, bool activeOnly = false, string? roleName = null)
        {
            try
            {
                IEnumerable<User> users = await _userRepository.GetAsync(includeProperties: "Role");

                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    users = users.Where(user => user.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    users = users.Where(user => user.Role.RoleName.Contains(roleName, StringComparison.OrdinalIgnoreCase));
                }

                if (activeOnly)
                {
                    users = users.Where(user => user.Status == StatusEnums.Active.ToString());
                }

                List<GetUserResponse> result = _mapper.Map<List<GetUserResponse>>(users);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserAsync(int key, UpdateUser updateUser)
        {
            try
            {
                User existedUser = await _userRepository.GetByIDAsync(key);

                if (existedUser == null)
                {
                    throw new Exception("User Id does not exist in the system.");
                }


                if (!string.IsNullOrEmpty(updateUser.FullName))
                {
                    existedUser.FullName = updateUser.FullName;
                }
                existedUser.Password = updateUser.Password;
                existedUser.Email = updateUser.Email;
                existedUser.PhoneNumber = updateUser.PhoneNumber;
                existedUser.Address = updateUser.Address;
                /* existedUser.ProfileImageUrl = updateUser.ProfileImageUrl;
                 existedUser.ImageMomoUrl = updateUser.ImageMomoUrl;*/


                if (!string.IsNullOrEmpty(updateUser.Status))
                {
                    if (updateUser.Status != "Active" && updateUser.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedUser.Status = updateUser.Status;
                }
                existedUser.UpdateDate = DateTime.Now;
                if (updateUser.UploadFile == null)
                {
                    existedUser.ProfileImageUrl = "";
                }
                else if (updateUser.UploadFile != null) existedUser.ProfileImageUrl = await _fileService.UploadFile(updateUser.UploadFile);



                await _userRepository.UpdateAsync(existedUser);
                await _userRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
