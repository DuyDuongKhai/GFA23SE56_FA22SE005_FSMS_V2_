using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Orders;

namespace FSMS.Service.Configs
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            //CreateMap<Order, GetOrder>().ReverseMap();
            //CreateMap<Order, GetSpecificOrder>().ReverseMap();

            CreateMap<Order, GetOrder>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
                               .IncludeMembers(src => src.User)
                               .ReverseMap();
            CreateMap<Order, GetOrder>().ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.User.Email))
                               .IncludeMembers(src => src.User)
                               .ReverseMap();
            CreateMap<User, GetOrder>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                         .ReverseMap();
            CreateMap<User, GetOrder>().ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                         .ReverseMap();

        }
    }
}
