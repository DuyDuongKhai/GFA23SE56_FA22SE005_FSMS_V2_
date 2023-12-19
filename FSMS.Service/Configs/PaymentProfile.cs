using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.CropVariety;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, GetPayment>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.User.FullName))
                                           .IncludeMembers(src => src.User)
                                           .ReverseMap();
            CreateMap<User, GetPayment>().ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                         .ReverseMap();
        }
    }
}
