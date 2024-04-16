using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Base
{
    public class ToggleEnableCommandValidator<TEntity> : AbstractValidator<ToggleEnableCommand<TEntity>>
    where TEntity : BaseEntity, IIsEnabled
    {
        public ToggleEnableCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
