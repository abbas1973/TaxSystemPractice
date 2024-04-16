using Application.Validators;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Cities
{
    public class CityGetListQueryValidator : AbstractValidator<CityGetListQuery>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public CityGetListQueryValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(_errorMaxLength);
        }
    }
}
