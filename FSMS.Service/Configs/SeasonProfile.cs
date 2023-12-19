using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<Season, GetSeason>()
                .ForMember(dest => dest.GardenName, opts => opts.MapFrom(src => src.Garden.GardenName))
                .IncludeMembers(src => src.Garden)
                .ReverseMap();

            CreateMap<Garden, GetSeason>() 
                .ForMember(dest => dest.GardenName, opts => opts.MapFrom(src => src.GardenName))
                .ReverseMap(); 
        }
    }
}
