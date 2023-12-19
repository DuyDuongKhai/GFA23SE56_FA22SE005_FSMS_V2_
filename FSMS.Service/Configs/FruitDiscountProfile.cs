using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.FruitDiscounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class FruitDiscountProfile : Profile
    {
        public FruitDiscountProfile()
        {
            CreateMap<FruitDiscount, GetFruitDiscount>()
               .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.Fruit.FruitName))
               .ReverseMap();

            CreateMap<Fruit, GetFruitDiscount>()
                .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.FruitName))
                .ReverseMap();
        }
    }
}
