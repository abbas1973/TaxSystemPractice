using Application.Validators;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Provinces
{
    public class ProvinceGetListQueryValidator : AbstractValidator<ProvinceGetListQuery>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public ProvinceGetListQueryValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(_errorMaxLength);
        }
    }
}
