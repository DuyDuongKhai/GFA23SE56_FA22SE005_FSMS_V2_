using FluentValidation;
using FSMS.Service.ViewModels.Seasons;
using System.Text.RegularExpressions;

namespace FSMS.Service.Validations.Season
{
    public class SeasonValidator : AbstractValidator<CreateSeason>
    {
        public SeasonValidator()
        {
            RuleFor(o => o.SeasonName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(50).WithMessage("{PropertyName} must be less than or equals 50 characters.");
            RuleFor(o => o.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(4000).WithMessage("{PropertyName} must be less than or equals 4000 characters.");
            RuleFor(o => o.StartDate)
                .Must(IsValidDate).WithMessage("Invalid {PropertyName}, The time gap must be around 30 year from the present and not exceeding 10 years");
            RuleFor(o => o.EndDate)
               .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be greater than StartDate")
                .Must(IsFirstDateAfterSecondDate).WithMessage("Invalid {PropertyName}, The time must over from the present");
            RuleFor(o => o.GardenId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            /* RuleFor(o => o.Image)
                 .Must(IsValidImageExtension).WithMessage("Invalid {PropertyName}, The accepted formats are: .jpeg, .png, .bmp, .webp");
 */
        }
        protected bool IsValidDate(DateTime taskDate)
        {
            int yearInput = taskDate.Year;
            int yearNow = DateTime.UtcNow.Year;
            if (yearInput > yearNow - 30 && yearInput < yearNow + 10)
                return true;
            return false;
        }
        protected bool IsFirstDateAfterSecondDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            return date > currentDate;
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
