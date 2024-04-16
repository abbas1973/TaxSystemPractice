using Application.Repositories;
using Application.Validators;
using FluentValidation;
using Resources;
using Utilities;

namespace Application.Features.Web.Users
{
    public class UserChangePasswordCommandValidator : AbstractValidator<UserChangePasswordCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public UserChangePasswordCommandValidator(IUnitOfWork uow)
        {
            Include(new IBaseEntityDTOValidator());
            Include(new IPasswordDTOValidator());

            RuleFor(x => x.RePassword)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(150).WithMessage(_errorMaxLength)
                .Equal(x => x.Password).WithMessage("{PropertyName} صحیح نیست!");

            //RuleFor(x => x.CurrentPassword)
            //    .NotEmpty().WithMessage(_errorRequired)
            //    .MaximumLength(150).WithMessage(_errorMaxLength)
            //    .MustAsync(async (model, currentPassword, cancellation) =>
            //    {
            //        var hashedPassword = currentPassword.GetHash();
            //        return await uow.Users.AnyAsync(x => x.Id == model.Id && x.Password == hashedPassword);
            //    }).WithMessage("{PropertyName} صحیح نیست!");

        }
    }
}
