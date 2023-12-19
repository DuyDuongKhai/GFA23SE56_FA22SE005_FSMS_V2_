using FluentValidation;
using FSMS.Service.ViewModels.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Validations.Payment
{
    public class ProcessPaymentValidator : AbstractValidator<ProcessPaymentRequest>
    {
        public ProcessPaymentValidator()
        {
            RuleFor(p => p.Status)
                .Must(s => s == "Completed" || s == "Failed" || s == "Refunded")
                .WithMessage("Status must be 'Completed' or 'Failed' or 'Refunded'");
        }
    }
}
