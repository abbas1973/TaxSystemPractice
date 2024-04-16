using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Users
{
    public class UserDeleteCommandValidator : AbstractValidator<UserDeleteCommand>
    {
        public UserDeleteCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
