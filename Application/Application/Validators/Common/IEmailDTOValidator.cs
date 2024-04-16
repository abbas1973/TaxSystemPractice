using Application.DTOs;
using FluentValidation;
using Resources;

namespace Application.Validators
{
    public class IEmailDTOValidator : AbstractValidator<IEmailDTO>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");

        public IEmailDTOValidator()
        {
            RuleFor(x => x.Email)
                //.NotNull().WithMessage(_errorRequired)
                //.NotEmpty().WithMessage(_errorRequired)
                .Must(BaseValidator.BeAValidEmail).WithMessage(string.Format(Messages.ErrorFormat, "{PropertyName}"));

        }
    }
}
