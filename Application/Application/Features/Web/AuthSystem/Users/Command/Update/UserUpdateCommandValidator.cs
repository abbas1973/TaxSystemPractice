using Application.Repositories;
using Application.Validators;
using FluentValidation;
using Resources;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Application.Features.Web.Users
{
    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public UserUpdateCommandValidator(IUnitOfWork uow)
        {
            Include(new IBaseEntityDTOValidator());
            Include(new IUsernameDTOValidator());
            Include(new IMobileDTOValidator());

            RuleFor(x => x.Username)
            .MustAsync(async (model, field, cancellation) =>
            {
                    var isExist = await uow.Users.AnyAsync(x => x.Id != model.Id && x.Username == field);
                    return !isExist;
                }).WithMessage("{PropertyName} تکراری است!");


            RuleFor(x => x.Mobile)
                .MustAsync(async (model, field, cancellation) =>
                {
                    var isExist = await uow.Users.AnyAsync(x => x.Id != model.Id && x.Mobile == field);
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

            RuleFor(x => x.RoleIds)
                .NotNull().WithMessage(_errorRequired)
                .Must(roleIds => roleIds.Any()).WithMessage("حداقل یک نقش را برای کاربر مشخص کنید.");

        }
    }
}
