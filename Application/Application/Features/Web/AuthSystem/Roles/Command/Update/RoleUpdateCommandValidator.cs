using Application.Repositories;
using Application.Validators;
using FluentValidation;
using Resources;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Application.Features.Web.Roles
{
    public class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public RoleUpdateCommandValidator(IUnitOfWork uow)
        {
            Include(new IBaseEntityDTOValidator());

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_errorRequired)
                .MaximumLength(200).WithMessage(_errorMaxLength);

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage(_errorMaxLength);

            RuleFor(x => x.Claims)
                .NotNull().WithMessage(_errorRequired)
                .Must(claims => claims.Any()).WithMessage("حداقل یک دسترسی برای نقش مشخص کنید.");

        }
    }
}
