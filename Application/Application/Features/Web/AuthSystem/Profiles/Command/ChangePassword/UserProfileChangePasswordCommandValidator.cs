using Application.Repositories;
using Application.SessionServices;
using Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Resources;
using Utilities;

namespace Application.Features.Web.Profiles
{
    public class UserProfileChangePasswordCommandValidator : AbstractValidator<UserProfileChangePasswordCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public UserProfileChangePasswordCommandValidator(IUnitOfWork uow, IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext.Session.GetUser();

            Include(new IPasswordDTOValidator());

            RuleFor(x => x.Captcha)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(10).WithMessage(_errorMaxLength);

            RuleFor(x => x.RePassword)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(150).WithMessage(_errorMaxLength)
                .Equal(x => x.Password).WithMessage("{PropertyName} صحیح نیست!");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(150).WithMessage(_errorMaxLength)
                .MustAsync(async (model, currentPassword, cancellation) =>
                {
                    var hashedPassword = currentPassword.GetHash();
                    return await uow.Users.AnyAsync(x => x.Id == user.Id && x.Password == hashedPassword);
                }).WithMessage("{PropertyName} صحیح نیست!");

        }
    }
}
