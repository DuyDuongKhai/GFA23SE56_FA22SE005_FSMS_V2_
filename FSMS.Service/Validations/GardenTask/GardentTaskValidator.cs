using FluentValidation;
using FSMS.Service.ViewModels.GardenTasks;
using System.Text.RegularExpressions;

namespace FSMS.Service.Validations.GardenTask
{
    public class GardentTaskValidator : AbstractValidator<CreateGardenTask>
    {

        public GardentTaskValidator()
        {
            RuleFor(o => o.GardenTaskName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(1, 100).WithMessage("{PropertyName} must be less than or equals 100 characters.");
            RuleFor(o => o.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(4000).WithMessage("{PropertyName} must be less than or equals 4000 characters.");
            RuleFor(o => o.GardenTaskDate)
                .Must(IsValidDate).WithMessage("Invalid {PropertyName}, The time gap must be around 50 year from the present and not exceeding 20 years");
            RuleFor(o => o.GardenId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            /*  RuleFor(o => o.Image)
                  .Must(IsValidImageExtension).WithMessage("Invalid {PropertyName}, The accepted formats are: .jpeg, .png, .bmp, .webp");
  */
        }
        protected bool IsValidDate(DateTime taskDate)
        {
            int yearInput = taskDate.Year;
            int yearNow = DateTime.UtcNow.Year;
            if (yearInput > yearNow - 50 && yearInput < yearNow + 20)
                return true;
            return false;
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
