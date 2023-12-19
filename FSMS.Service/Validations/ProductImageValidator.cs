using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSMS.Service.Validations
{
    public class ProductImageValidator
    {

    } /*: AbstractValidator<CreateProductImage>
    {
        public ProductImageValidator()
        {            
           
            RuleFor(o => o.ProductId)
               .NotEmpty().WithMessage("{PropertyName} is empty");
        }
        protected bool IsValidImageExtension(string filename)
        {
            string validExtensionsPattern = @"\.(jpeg|png|bmp|webp)$";
            if (Regex.IsMatch(filename, validExtensionsPattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
    
}
