using FluentValidation;
using FSMS.Service.ViewModels.Posts;
using System.Text.RegularExpressions;

namespace FSMS.Service.Validations.Post
{
    public class PostValidator : AbstractValidator<CreatePost>
    {
        public PostValidator()
        {
            RuleFor(o => o.PostTitle)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("{PropertyName} is empty")
            .Length(1, 100).WithMessage("{PropertyName} must be less than or equals 100 characters.");
            RuleFor(o => o.PostContent)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("{PropertyName} is empty")
            .Length(1, 4000).WithMessage("{PropertyName} must be less than or equals 4000 characters.");
            RuleFor(o => o.Type)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(1, 50).WithMessage("{PropertyName} must be less than or equals 50 characters.");
            /*  RuleFor(o => o.PostImage)
                  .Must(IsValidImageExtension).WithMessage("Invalid {PropertyName}, The accepted formats are: .jpeg, .png, .bmp, .webp");*/
            RuleFor(o => o.UserId)
               .NotEmpty().WithMessage("{PropertyName} is empty");
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
