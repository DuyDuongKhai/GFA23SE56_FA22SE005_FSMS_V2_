using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, GetPost>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
                                           .IncludeMembers(src => src.User)
                                           .ReverseMap();
            CreateMap<User, GetPost>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                         .ReverseMap();
        }
    }
}
