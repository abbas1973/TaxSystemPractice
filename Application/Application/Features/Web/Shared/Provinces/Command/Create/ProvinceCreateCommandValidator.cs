using Application.Repositories;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Provinces
{
    public class ProvinceCreateCommandValidator : AbstractValidator<ProvinceCreateCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public ProvinceCreateCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(100).WithMessage(_errorMaxLength);

        }
    }
}
