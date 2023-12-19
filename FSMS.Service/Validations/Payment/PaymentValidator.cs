using FluentValidation;
using FSMS.Service.ViewModels.Payments;

namespace FSMS.Service.Validations.Payment
{
    public class PaymentValidator : AbstractValidator<CreatePayment>
    {
        public PaymentValidator()
        {
            RuleFor(o => o.OrderId)
                .NotEmpty().WithMessage("{PropertyName} is empty");



            RuleFor(o => o.UserId)
               .NotEmpty().WithMessage("{PropertyName} is empty");
        }
        protected bool IsFirstDateAfterSecondDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;

            return date > currentDate;
        }
    }
}
