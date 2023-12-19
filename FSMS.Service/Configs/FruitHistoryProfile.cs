using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.FruitHistories;
using FSMS.Service.ViewModels.Gardens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class FruitHistoryProfile : Profile
    {
        public FruitHistoryProfile()
        {
            CreateMap<FruitHistory, GetFruitHistory>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
                               .IncludeMembers(src => src.User)
                               .ReverseMap();

            CreateMap<User, GetFruitHistory>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                         .ReverseMap();
        }
    }
}
