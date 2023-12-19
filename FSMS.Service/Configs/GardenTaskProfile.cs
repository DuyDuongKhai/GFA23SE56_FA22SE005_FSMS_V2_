using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.GardenTasks;

namespace FSMS.Service.Configs
{
    public class GardenTaskProfile : Profile
    {
        public GardenTaskProfile()
        {
            CreateMap<GardenTask, GetGardenTask>()
                .ForMember(dest => dest.GardenName, opts => opts.MapFrom(src => src.Garden.GardenName))
                .ForMember(dest => dest.PlantName, opts => opts.MapFrom(src => src.Plant.PlantName))
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Garden.UserId))
                .ReverseMap();
        }
    }
}
