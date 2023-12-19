using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.CategoryFruits
{
    public class CreateCategoryFruit
    {
        [Required(ErrorMessage = "Category Name  is required.")]
        [MaxLength(200, ErrorMessage = "Category Name  must be less than or equals 200 characters.")]
        public string CategoryFruitName { get; set; }
    }
}
