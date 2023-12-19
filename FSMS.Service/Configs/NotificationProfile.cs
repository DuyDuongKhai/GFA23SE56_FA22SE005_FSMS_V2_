using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, GetNotification>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
                                           .IncludeMembers(src => src.User)
                                           .ReverseMap();
            CreateMap<User, GetNotification>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                         .ReverseMap();
        }
    }
}
