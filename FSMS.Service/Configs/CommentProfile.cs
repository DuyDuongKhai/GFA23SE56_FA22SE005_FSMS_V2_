using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Comments;
using FSMS.Service.ViewModels.Plants;
using FSMS.Service.ViewModels.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, GetComment>()
               .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.PostContent, opts => opts.MapFrom(src => src.Post.PostContent))
               .ReverseMap();

            CreateMap<User, GetComment>()
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                .ReverseMap();

            CreateMap<Post, GetComment>()
                .ForMember(dest => dest.PostContent, opts => opts.MapFrom(src => src.PostContent))
                .ReverseMap();
        }
    }
}
