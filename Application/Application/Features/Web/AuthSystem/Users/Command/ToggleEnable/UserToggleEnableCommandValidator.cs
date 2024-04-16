using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Users
{
    public class UserToggleEnableCommandValidator : AbstractValidator<UserToggleEnableCommand>
    {
        public UserToggleEnableCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
