using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.CropVariety;

namespace FSMS.Service.Configs
{
    public class CropVarietyProfile : Profile
    {
        public CropVarietyProfile()
        {
            CreateMap<CropVariety, GetCropVariety>();
        }
    }
}
