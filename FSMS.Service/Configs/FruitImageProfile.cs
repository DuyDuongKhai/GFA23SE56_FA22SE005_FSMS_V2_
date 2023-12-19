using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.FruitImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class FruitImageProfile : Profile
    {
        public FruitImageProfile()
        {
            CreateMap<FruitImage, GetFruitImage>()
                .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.Fruit.FruitName))
                .IncludeMembers(src => src.Fruit)
                .ReverseMap();

            CreateMap<Fruit, GetFruitImage>()
                .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.FruitName))
                .ReverseMap();
            CreateMap<FruitImage, CreateFruitImage>()
                .ReverseMap();
            CreateMap<FruitImage, UpdateFruitImage>()
                .ReverseMap();
            /*   CreateMap<ProductImage, GetProductImage>()
                   .ReverseMap();*/
        }
    }
}
