using Application.Validators;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Roles
{
    public class RoleGetListQueryValidator : AbstractValidator<RoleGetListQuery>
    {
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");

        public RoleGetListQueryValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage(_errorMaxLength);
        }
    }
}
