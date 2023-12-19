using FluentValidation;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Orders;
using System.Text.RegularExpressions;

namespace FSMS.Service.Validations.Order
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrder>
    {
        public UpdateOrderValidator()
        {
            RuleFor(o => o.DeliveryAddress)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(o => o.PaymentMethod)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .IsEnumName(typeof(PaymentMethodEnum)).WithMessage("{PropertyName} must be Momo or COD");
            RuleFor(o => o.PhoneNumber)
               .Must(IsValidPhoneNumber).WithMessage("{PropertyName}  must start with one of the following prefixes: 032-039, 070-079, 081-089, 090-099, or 012-019. and must be 10 or 11 digits long.");
            RuleFor(o => o.Status)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .IsEnumName(typeof(OrderEnum)).WithMessage("{PropertyName} must be Pending, Accepted, Rejected, Cancelled, Shipping, UserRefused");
        }

        protected bool IsValidPhoneNumber(string inputPhoneNumber)
        {
            string phoneNumberPattern = @"^(03[2-9]|07[0|6-9]|08[1-9]|09[0-9]|01[2|6-9])+([0-9]{7,8})\b";
            return Regex.IsMatch(inputPhoneNumber, phoneNumberPattern);
        }
    }
}
