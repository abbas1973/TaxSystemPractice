using Application.DTOs;
using FluentValidation;
using Resources;

namespace Application.Validators
{
    public class IPasswordDTOValidator : AbstractValidator<IPasswordDTO>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");

        public IPasswordDTOValidator()
        {
            RuleFor(x => x.Password).NotNull().WithMessage(_errorRequired)
                .NotEmpty().WithMessage(_errorRequired)
                .MinimumLength(6).WithMessage(string.Format(Messages.ErrorMinLength, "{PropertyName}", "{MinLength}"))
                .MaximumLength(150).WithMessage(string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}"))
                .Must(BaseValidator.BeAValidPassword).WithMessage(string.Format(Messages.ErrorPassword, "{PropertyName}"));
        }
    }
}
