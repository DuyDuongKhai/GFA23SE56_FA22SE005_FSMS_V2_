using FluentValidation;
using FSMS.Service.ViewModels.FruitDiscounts;

namespace FSMS.Service.Validations.FruitDiscount
{
    public class FruitDiscountValidator : AbstractValidator<CreateFruitDiscount>
    {
        public FruitDiscountValidator()
        {

            RuleFor(o => o.FruitId)
              .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(o => o.DiscountName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(200).WithMessage("{PropertyName} must be less than or equals 200 characters.");
            RuleFor(o => o.DiscountThreshold)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("{PropertyName} must be less than 100");
            RuleFor(o => o.DiscountPercentage)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(1).WithMessage("{PropertyName} must be less than 1");
            RuleFor(o => o.DiscountExpiryDate)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .Must(IsFirstDateAfterSecondDate).WithMessage("Invalid {PropertyName}, The time must over from the present");

        }
        protected bool IsFirstDateAfterSecondDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;

            return date > currentDate;
        }
    }
}
