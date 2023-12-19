using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.OrderDetails;

namespace FSMS.Service.Configs
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, GetOrderDetail>()
                .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.Fruit.FruitName))
                .ForMember(dest => dest.DiscountName, opts => opts.MapFrom(src => src.FruitDiscount.DiscountName))
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Fruit.UserId))
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.Fruit.User.FullName))
                .IncludeMembers(src => src.Fruit, src => src.FruitDiscount, src => src.Fruit.User)
                .ReverseMap();

            CreateMap<Fruit, GetOrderDetail>()
                .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.FruitName))
                .ReverseMap();

            CreateMap<FruitDiscount, GetOrderDetail>()
                .ForMember(dest => dest.DiscountName, opts => opts.MapFrom(src => src.DiscountName))
                .ReverseMap();

            CreateMap<User, GetOrderDetail>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                .ReverseMap();
        }
    }
}
