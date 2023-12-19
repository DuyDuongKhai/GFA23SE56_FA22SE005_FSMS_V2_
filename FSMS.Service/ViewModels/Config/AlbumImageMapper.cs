using AutoMapper;
using FSMS.Service.ViewModels.FruitImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Config
{
    public static class AlbumImageMapper
    {
        public static void ConfigAlbumImage(this IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<FSMS.Entity.Models.FruitImage, GetFruitImage>().ReverseMap();
            configuration.CreateMap<FSMS.Entity.Models.FruitImage, CreateFruitImage>().ReverseMap();
            configuration.CreateMap<FSMS.Entity.Models.FruitImage, UpdateFruitImage>().ReverseMap();
        }
    }
}
