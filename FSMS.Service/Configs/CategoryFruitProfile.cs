using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.CategoryFruits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class CategoryFruitProfile : Profile
    {
        public CategoryFruitProfile()
        {
            CreateMap<CategoryFruit, GetCategoryFruit>();
        }
    }
}
