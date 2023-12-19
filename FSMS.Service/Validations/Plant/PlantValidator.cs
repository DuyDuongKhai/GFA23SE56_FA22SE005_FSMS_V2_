using FluentValidation;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Plants;
using System.Text.RegularExpressions;

namespace FSMS.Service.Validations.Plant
{
    public class PlantValidator : AbstractValidator<CreatePlant>
    {
        public PlantValidator()
        {
            RuleFor(o => o.PlantName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(200).WithMessage("{PropertyName} must be less than or equals 200 characters.");
            RuleFor(o => o.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(4000).WithMessage("{PropertyName} must be less than or equals 4000 characters.");
            RuleFor(o => o.PlantingDate)
                .Must(IsValidDate).WithMessage("Invalid {PropertyName}, The time gap must be around 30 year from the present and not exceeding 10 years");
            RuleFor(o => o.HarvestingDate)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(x => x.PlantingDate).WithMessage("HarvestingDate must be greater than or equal to PlantingDate")
                .Must(IsFirstDateAfterSecondDate).WithMessage("Invalid {PropertyName}, The time must over from the present");
            RuleFor(o => o.GardenId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(o => o.CropVarietyId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            /*   RuleFor(o => o.Image)
                   .Must(IsValidImageExtension).WithMessage("Invalid {PropertyName}, The accepted formats are: .jpeg, .png, .bmp, .webp");*/
            RuleFor(o => o.QuantityPlanted)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than or equal 0");
            RuleFor(g => g.Status)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .IsEnumName(typeof(PlantEnum)).WithMessage("{PropertyName} must be Seed or GerminatedSeed or  Seedling or YoungTree or FloweringTree or Harvestable or Diseased or Dead");



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
