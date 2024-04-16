using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Roles
{
    public class RoleDeleteCommandValidator : AbstractValidator<RoleDeleteCommand>
    {
        public RoleDeleteCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
