using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Users;

namespace FSMS.Service.Configs
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUser>().ForMember(dept => dept.RoleName, opts => opts.MapFrom(src => src.Role.RoleName)).ReverseMap();
            CreateMap<User, GetUserResponse>().ForMember(dept => dept.RoleName, opts => opts.MapFrom(src => src.Role.RoleName)).ReverseMap();
            CreateMap<User, SignInAccount>();
        }
    }
}
