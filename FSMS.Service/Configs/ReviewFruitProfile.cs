using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.ReviewFruits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class ReviewFruitProfile : Profile
    {
        public ReviewFruitProfile()
        {
            CreateMap<ReviewFruit, GetReviewFruit>()
               .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.Fruit.FruitName))
               .ReverseMap();

            CreateMap<User, GetReviewFruit>()
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                .ReverseMap();

            CreateMap<Fruit, GetReviewFruit>()
                .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.FruitName))
                .ReverseMap();
        }
    }
}
