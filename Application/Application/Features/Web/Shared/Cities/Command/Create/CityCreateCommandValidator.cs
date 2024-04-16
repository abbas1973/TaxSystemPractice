using Application.Repositories;
using Application.Validators;
using FluentValidation;
using Resources;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Application.Features.Web.Cities
{
    public class CityCreateCommandValidator : AbstractValidator<CityCreateCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public CityCreateCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(100).WithMessage(_errorMaxLength);

            RuleFor(x => x.ProvinceId)
                .GreaterThan(0).WithMessage(_errorGreaterThan);

        }
    }
}
