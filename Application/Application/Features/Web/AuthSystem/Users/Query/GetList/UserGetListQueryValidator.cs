using Application.Validators;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Users
{
    public class UserGetListQueryValidator : AbstractValidator<UserGetListQuery>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public UserGetListQueryValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage(_errorMaxLength);

            RuleFor(x => x.Mobile)
                .MaximumLength(11).WithMessage(_errorMaxLength);
        }
    }
}
