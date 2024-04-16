using Application.Validators;
using FluentValidation;

namespace Application.Features.Web.Invoices
{
    public class InvoiceGetListQueryValidator : AbstractValidator<InvoiceGetListQuery>
    {
        public InvoiceGetListQueryValidator()
        {
            RuleFor(x => x.TaxId)
                .MaximumLength(100).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.InvoiceNumber)
                .MaximumLength(50).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerName)
                .MaximumLength(100).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerNationalCode)
                .MaximumLength(15).WithMessage(ValidatorErrors.MaxLength);
        }
    }
}
