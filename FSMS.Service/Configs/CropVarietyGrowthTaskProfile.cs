using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.CropVarietyGrowthTasks;

namespace FSMS.Service.Configs
{
    public class CropVarietyGrowthTaskProfile : Profile
    {
        public CropVarietyGrowthTaskProfile()
        {
            CreateMap<CropVarietyGrowthTask, GetCropVarietyGrowthTask>()
            .ForMember(dest => dest.StageName, opts => opts.MapFrom(src => src.CropVarietyStage.StageName))
            .ReverseMap();
        }
    }
}
