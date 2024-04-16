using Application.DTOs;
using FluentValidation;
using Resources;

namespace Application.Validators
{
    public class IUsernameDTOValidator : AbstractValidator<IUsernameDTO>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");

        public IUsernameDTOValidator()
        {
            RuleFor(x => x.Username).NotNull().WithMessage(_errorRequired)
                .NotEmpty().WithMessage(_errorRequired)
                .MinimumLength(5).WithMessage(string.Format(Messages.ErrorMinLength, "{PropertyName}", "{MinLength}"))
                .MaximumLength(30).WithMessage(string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}"))
                .Must(BaseValidator.BeAValidUsername).WithMessage(string.Format(Messages.ErrorUsername, "{PropertyName}"));

        }
    }
}
