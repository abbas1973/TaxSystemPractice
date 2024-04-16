using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Cities
{
    public class CityDeleteCommandValidator : AbstractValidator<CityDeleteCommand>
    {
        public CityDeleteCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
