using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Fruits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class FruitProfile : Profile
    {
        public FruitProfile()
        {
            CreateMap<Fruit, GetFruitFarmer>()
               .ForMember(dest => dest.CategoryFruitName, opts => opts.MapFrom(src => src.CategoryFruit.CategoryFruitName))
               .ForMember(dest => dest.PlantName, opts => opts.MapFrom(src => src.Plant.PlantName))
               .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
               .ReverseMap();

            CreateMap<CategoryFruit, GetFruitFarmer>()
                .ForMember(dest => dest.CategoryFruitName, opts => opts.MapFrom(src => src.CategoryFruitName))
                .ReverseMap();

            CreateMap<Plant, GetFruitFarmer>()
                .ForMember(dest => dest.PlantName, opts => opts.MapFrom(src => src.PlantName))
                .ReverseMap();
            CreateMap<User, GetFruitFarmer>()
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                .ReverseMap();


            CreateMap<Fruit, GetFruitSupplier>()
                .ForMember(dest => dest.CategoryFruitName, opts => opts.MapFrom(src => src.CategoryFruit.CategoryFruitName))
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
                .ReverseMap();

            CreateMap<CategoryFruit, GetFruitSupplier>()
                .ForMember(dest => dest.CategoryFruitName, opts => opts.MapFrom(src => src.CategoryFruitName))
                .ReverseMap();

            CreateMap<User, GetFruitSupplier>()
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                .ReverseMap();

        }
    }
}
