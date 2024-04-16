using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Roles
{
    public class RoleToggleEnableCommandValidator : AbstractValidator<RoleToggleEnableCommand>
    {
        public RoleToggleEnableCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
