using Application.Repositories;
using Application.SessionServices;
using Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Resources;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Application.Features.Web.Profiles
{
    public class UserProfileUpdateCommandValidator : AbstractValidator<UserProfileUpdateCommand>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorMaxLength = string.Format(Messages.ErrorMaxLength, "{PropertyName}", "{MaxLength}");
        private string _errorGreaterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public UserProfileUpdateCommandValidator(IUnitOfWork uow, IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext.Session.GetUser();

            Include(new IUsernameDTOValidator());
            Include(new IMobileDTOValidator());

            RuleFor(x => x.Username)
            .MustAsync(async (model, field, cancellation) =>
            {
                    var isExist = await uow.Users.AnyAsync(x => x.Id != user.Id && x.Username == field);
                    return !isExist;
                }).WithMessage("{PropertyName} تکراری است!");


            RuleFor(x => x.Mobile)
                .MustAsync(async (model, field, cancellation) =>
                {
                    var isExist = await uow.Users.AnyAsync(x => x.Id != user.Id && x.Mobile == field);
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


        }
    }
}
