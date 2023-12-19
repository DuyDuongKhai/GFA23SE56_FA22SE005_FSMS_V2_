using FluentValidation;
using FSMS.Service.ViewModels.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Validations.Order
{
    internal class OrderDetailValidation : AbstractValidator<CreateOrderDetail>
    {
        public OrderDetailValidation()
        {

            RuleFor(o => o.FruitId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(o => o.Quantity)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("{PropertyName} is empty")
              .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be greater than or equals 1.");
           
        }
    }
}
