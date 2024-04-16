using Application.DTOs;
using FluentValidation;
using Resources;

namespace Application.Validators
{
    public class IPaginationDTOValidator : AbstractValidator<IPaginationDTO>
    {
        private string _errorRequired = string.Format(Messages.ErrorRequired, "{PropertyName}");
        private string _errorGraterThan = string.Format(Messages.ErrorGreaterThan, "{PropertyName}", "{ComparisonValue}");

        public IPaginationDTOValidator()
        {
            RuleFor(x => x.Page).NotNull().WithMessage(_errorRequired)
                .GreaterThan(0).WithMessage(_errorGraterThan);

            RuleFor(x => x.PageLength).NotNull().WithMessage(_errorRequired)
                .GreaterThan(0).WithMessage(_errorGraterThan);

        }
    }
}
