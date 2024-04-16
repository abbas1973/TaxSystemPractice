using Application.DTOs;
using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Base
{
    public class GetListQueryValidator<TEntity,Tout> : AbstractValidator<GetListQuery<TEntity, Tout>>
    where TEntity : IBaseEntity
    where Tout : class
    {
        public GetListQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage(ValidatorErrors.GreaterThan);

            RuleFor(x => x.PageLength)
                .GreaterThan(0).WithMessage(ValidatorErrors.GreaterThan);

            RuleFor(x => x.SortColumn)
                .MaximumLength(100).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.SortDirection)
                .MaximumLength(100).WithMessage(ValidatorErrors.MaxLength);
        }
    }

}
