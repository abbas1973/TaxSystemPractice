using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Base
{
    public class DeleteCommandValidator<TEntity> : AbstractValidator<DeleteCommand<TEntity>>
    where TEntity : BaseEntity
    {
        public DeleteCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
