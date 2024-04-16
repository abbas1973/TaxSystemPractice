using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Provinces
{
    public class ProvinceDeleteCommandValidator : AbstractValidator<ProvinceDeleteCommand>
    {
        public ProvinceDeleteCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
