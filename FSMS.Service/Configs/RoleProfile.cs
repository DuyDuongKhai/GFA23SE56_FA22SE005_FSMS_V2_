using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Comments;
using FSMS.Service.ViewModels.CropVariety;
using FSMS.Service.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
                CreateMap<Role, GetRole>();
        }
    }
}
