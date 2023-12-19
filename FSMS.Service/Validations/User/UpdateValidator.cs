using FluentValidation;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Users;
using System.Text.RegularExpressions;

namespace FSMS.Service.Validations.User
{
    public class UpdateValidator : AbstractValidator<UpdateUser>
    {
        public UpdateValidator()
        {
            RuleFor(o => o.Email)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .MaximumLength(50).WithMessage("{PropertyName} must be less than or equals 50 characters.")
               .EmailAddress();
            RuleFor(o => o.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(15).WithMessage("{PropertyName} must be less than or equals 15 characters.");
            RuleFor(o => o.FullName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(50).WithMessage("{PropertyName} must be less than or equals 15 characters.");
            RuleFor(o => o.PhoneNumber)
               .Must(IsValidPhoneNumber).WithMessage("{PropertyName}  must start with one of the following prefixes: 032-039, 070-079, 081-089, 090-099, or 012-019. and must be 10 or 11 digits long.");
            RuleFor(g => g.Status)
              .NotEmpty().WithMessage("{PropertyName} is empty")
              .IsEnumName(typeof(StatusEnums)).WithMessage("{PropertyName} must be Active or InActive");
            RuleFor(o => o.Address)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .MaximumLength(100).WithMessage("{PropertyName} must be less than or equals 100 characters.");
            /* RuleFor(o => o.ProfileImageUrl)
               .Must(IsValidImageExtension).WithMessage("{PropertyName} must be following image file extensions: jpeg, png, bmp, or webp");
 */

        }
        protected bool IsNameValid1(string name)
        {
            var regex = new Regex(@"^[a-zA-Z ]+$");
            if (!regex.IsMatch(name))
            {
                return false;
            }
            return true;
        }
        protected bool IsNameValid2(string name)
        {

            if (!name.Any(char.IsLetter))
            {
                return false;
            }

            return true;
        }
        protected bool IsNameValid3(string name)
        {
            string[] nameParts = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length < 2)
            {
                return false;
            }

            return true;
        }

        protected bool IsValidPhoneNumber(string inputPhoneNumber)
        {
            string phoneNumberPattern = @"^(03[2-9]|07[0|6-9]|08[1-9]|09[0-9]|01[2|6-9])+([0-9]{7,8})\b";
            return Regex.IsMatch(inputPhoneNumber, phoneNumberPattern);
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
