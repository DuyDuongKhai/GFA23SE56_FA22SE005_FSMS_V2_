using FluentValidation;
using FSMS.Service.ViewModels.Gardens;
using System.Text.RegularExpressions;

namespace FSMS.Service.Validations
{

    public class GardenValidator : AbstractValidator<CreateGarden>
    {
        public GardenValidator()
        {
            RuleFor(g => g.GardenName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("{PropertyName} must be less than or equals 50 characters.");
            RuleFor(g => g.Description)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .Length(2, 4000).WithMessage("{PropertyName} must be less than or equals 4000 characters.");
            RuleFor(g => g.Region)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .Length(2, 50).WithMessage("{PropertyName} must be less than or equals 50 characters.");
            RuleFor(g => g.UserId)
               .NotEmpty().WithMessage("{PropertyName} is empty");
            /* RuleFor(o => o.Image)
               .Must(IsValidImageExtension).WithMessage("Invalid {PropertyName}, The accepted formats are: .jpeg, .png, .bmp, .webp");
 */
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
        }
    }
}
