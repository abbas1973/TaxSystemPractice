using Application.Validators;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Web.Invoices
{
    public class InvoiceDeleteCommandValidator : AbstractValidator<InvoiceDeleteCommand>
    {
        public InvoiceDeleteCommandValidator()
        {
            Include(new IBaseEntityDTOValidator());
        }
    }
}
