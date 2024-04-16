using Application.DTOs;
using FluentValidation;
using Resources;

namespace Application.Validators
{
    public class IMobileDTOValidator : AbstractValidator<IMobileDTO>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");

        public IMobileDTOValidator()
        {
            RuleFor(x => x.Mobile).NotNull().WithMessage(_errorRequired)
                .NotEmpty().WithMessage(_errorRequired)
                .Length(11).WithMessage(string.Format(Messages.ErrorLength, "{PropertyName}", "{MaxLength}"))
                .Must(BaseValidator.BeAValidMobile).WithMessage(string.Format(Messages.ErrorFormat, "{PropertyName}"));

        }
    }
}
