using Application.Repositories;
using Application.Validators;
using FluentValidation;
using Resources;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Application.Features.Web.Users
{
    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public UserCreateCommandValidator(IUnitOfWork uow)
        {
            Include(new IUsernameDTOValidator());
            Include(new IPasswordDTOValidator());
            Include(new IMobileDTOValidator());

            RuleFor(x => x.Username)
                .MustAsync(async (model, field, cancellation) =>
                {
                    var isExist = await uow.Users.AnyAsync(x => x.Username == field);
                    return !isExist;
                }).WithMessage("{PropertyName} تکراری است!");


            RuleFor(x => x.Mobile)
                .MustAsync(async (model, field, cancellation) =>
                {
                    var isExist = await uow.Users.AnyAsync(x => x.Mobile == field);
                    return !isExist;
                }).WithMessage("{PropertyName} تکراری است!");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(100).WithMessage(_errorMaxLength);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(100).WithMessage(_errorMaxLength);

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage(_errorGreaterThan);

            RuleFor(x => x.RePassword)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(150).WithMessage(_errorMaxLength)
                .Equal(x => x.Password).WithMessage("{PropertyName} صحیح نیست!");

            RuleFor(x => x.RoleIds)
                .NotNull().WithMessage(_errorRequired)
                .Must(roleIds => roleIds.Any()).WithMessage("حداقل یک نقش را برای کاربر مشخص کنید.");

        }
    }
}
