using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.GardenTasks;
using FSMS.Service.ViewModels.Plants;
using FSMS.Service.ViewModels.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class PlantProfile : Profile
    {
        public PlantProfile()
        {
            CreateMap<Plant, GetPlant>()
                .ForMember(dest => dest.GardenName, opts => opts.MapFrom(src => src.Garden.GardenName))
                .ForMember(dest => dest.CropVarietyName, opts => opts.MapFrom(src => src.CropVariety.CropVarietyName))
                .ReverseMap();

            CreateMap<Garden, GetPlant>()
                .ForMember(dest => dest.GardenName, opts => opts.MapFrom(src => src.GardenName))
                .ReverseMap();
            CreateMap<CropVariety, GetPlant>()
                .ForMember(dest => dest.CropVarietyName, opts => opts.MapFrom(src => src.CropVarietyName))
                .ReverseMap();

        }
    }
}
