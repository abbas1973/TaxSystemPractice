using Application.Repositories;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Companies
{
    public class CompanyUpdateCommandValidator : AbstractValidator<CompanyUpdateCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public CompanyUpdateCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(100).WithMessage(_errorMaxLength);

            RuleFor(x => x.PrivateKey)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(3000).WithMessage(_errorMaxLength);

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(20).WithMessage(_errorMaxLength);

            RuleFor(x => x.EconomicCode)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(50).WithMessage(_errorMaxLength);

            RuleFor(x => x.NationalCode)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(11).WithMessage(_errorMaxLength);

        }
    }
}
