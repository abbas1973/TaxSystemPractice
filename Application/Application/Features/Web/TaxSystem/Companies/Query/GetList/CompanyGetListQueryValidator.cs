using Application.Validators;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Companies
{
    public class CompanyGetListQueryValidator : AbstractValidator<CompanyGetListQuery>
    {

        public CompanyGetListQueryValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.ClientId)
                .MaximumLength(20).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.EconomicCode)
                .MaximumLength(50).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.NationalCode)
                .MaximumLength(11).WithMessage(ValidatorErrors.MaxLength);
        }
    }
}
