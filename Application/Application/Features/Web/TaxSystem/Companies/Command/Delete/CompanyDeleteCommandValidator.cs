using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Companies
{
    public class CompanyDeleteCommandValidator : AbstractValidator<CompanyDeleteCommand>
    {
        public CompanyDeleteCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
