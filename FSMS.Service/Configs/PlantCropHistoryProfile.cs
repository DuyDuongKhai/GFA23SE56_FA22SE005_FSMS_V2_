using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.PlantCropHistory;

namespace FSMS.Service.Configs
{
    public class PlantCropHistoryProfile : Profile
    {
        public PlantCropHistoryProfile()
        {
            CreateMap<PlantCropHistory, GetPlantCropHistory>()
                .ForMember(dest => dest.PlantName, opt => opt.MapFrom(src => src.Plant.PlantName))
                .ForMember(dest => dest.GardenName, opt => opt.MapFrom(src => src.Plant.Garden.GardenName));
        }
    }
}
