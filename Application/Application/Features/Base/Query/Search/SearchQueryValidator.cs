using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Base
{
    public class SearchQueryValidator<TEntity> : AbstractValidator<SearchQuery<TEntity>>
        where TEntity : IBaseEntity
    {
        public SearchQueryValidator()
        {
            RuleFor(x => x.Text)
                .MaximumLength(100).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage(ValidatorErrors.GreaterThan)
                .LessThanOrEqualTo(100).WithMessage(ValidatorErrors.LessThanOrEqual);
        }
    }
}
