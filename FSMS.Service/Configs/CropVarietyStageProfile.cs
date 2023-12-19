using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.CropVarietyStages;
using FSMS.Service.ViewModels.GardenTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class CropVarietyStageProfile : Profile
    {
        public CropVarietyStageProfile()
        {
            CreateMap<CropVarietyStage, GetCropVarietyStage>()
            .ForMember(dest => dest.CropVarietyName, opts => opts.MapFrom(src => src.CropVariety.CropVarietyName))
            .ReverseMap();
        }
    }
}
