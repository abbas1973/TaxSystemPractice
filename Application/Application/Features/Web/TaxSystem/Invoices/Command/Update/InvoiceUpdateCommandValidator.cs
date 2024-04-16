using Application.Repositories;
using Application.Validators;
using DocumentFormat.OpenXml.Vml;
using Domain.Enums;
using FluentValidation;
using Resources;

namespace Application.Features.Web.Invoices
{
    public class InvoiceUpdateCommandValidator : AbstractValidator<InvoiceUpdateCommand>
    {
        public InvoiceUpdateCommandValidator()
        {
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(50).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerName)
                .NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(300).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerNationalCode)
                .NotEmpty().WithMessage(ValidatorErrors.Required)
                .Must(BaseValidator.BeAValidNationalCode)
                    .When(x => x.BuyerIsRealOrLegal == BuyerType.Real)
                    .WithMessage(ValidatorErrors.Format)
                .Must(BaseValidator.BeAValidNationalId)
                    .When(x => x.BuyerIsRealOrLegal == BuyerType.Legal)
                    .WithMessage(ValidatorErrors.Format)
                .MaximumLength(11).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerEconomicCode)
                .NotEmpty().When(x => x.BuyerIsRealOrLegal == BuyerType.Legal).WithMessage(ValidatorErrors.Required)
                .MaximumLength(50).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerMobile)
                //.NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(11).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerAddress)
                .NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(1500).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerPostalCode)
                .NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(10).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.BuyerPhone)
                //.NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(11).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.Description)
                //.NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(1000).WithMessage(ValidatorErrors.MaxLength);


            RuleFor(x => x.TaxInvoiceType)
                .Must((model, field) =>
                {
                    if (field == TaxInvoiceType.Type2)
                        return model.TaxInvoicePattern is TaxInvoicePattern.Pattern1 or TaxInvoicePattern.Pattern3;
                    return true;
                });

            RuleFor(x => x.ContractId)
                .NotEmpty().When(x => x.TaxInvoicePattern == TaxInvoicePattern.Pattern4).WithMessage(ValidatorErrors.Required)
                .MaximumLength(12).WithMessage(ValidatorErrors.MaxLength);


            RuleFor(x => x.CashAmount)
                .GreaterThanOrEqualTo(0).WithMessage(ValidatorErrors.Required);

            RuleFor(x => x.InvoiceItems)
                .NotNull().WithMessage(ValidatorErrors.Required)
                .Must((model, field) =>
                {
                    return field != null &&  field.Any();
                }).WithMessage(ValidatorErrors.Required);

            RuleFor(x => x.PayType)
                .Must((model, field) =>
                {
                    if (model.InvoiceItems == null)
                        return true;
                    var total = model.InvoiceItems.Sum(x => (x.Quantity * x.UnitPrice) - x.DiscountAmount + x.TaxAmount + x.OtherTaxAmount);

                    if (field == InvoicePayType.Cash)
                        return model.CashAmount == total;
                    else
                        return model.CashAmount < total;
                }).WithMessage("مبلغ پرداخت نقدی در حالت نقدی باید برابر با کل مبلغ و در حالت نسیه باید کمتر از مبلغ کل باشد.");

            RuleForEach(x => x.InvoiceItems)
                .SetValidator(new InvoiceItemUpdateDTOValidator());
        }
    }







    public class InvoiceItemUpdateDTOValidator : AbstractValidator<InvoiceItemUpdateDTO>
    {
        public InvoiceItemUpdateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(300).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(50).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.CountingUnitName)
                //.NotEmpty().WithMessage(ValidatorErrors.Required)
                .MaximumLength(100).WithMessage(ValidatorErrors.MaxLength);

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage(ValidatorErrors.Required);

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage(ValidatorErrors.Required);

            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage(ValidatorErrors.Required);

            RuleFor(x => x.TaxRate)
                .GreaterThanOrEqualTo(0).WithMessage(ValidatorErrors.Required);

            RuleFor(x => x.TaxAmount)
                .GreaterThanOrEqualTo(0).WithMessage(ValidatorErrors.Required);

            RuleFor(x => x.OtherTaxAmount)
                .GreaterThanOrEqualTo(0).WithMessage(ValidatorErrors.Required);


        }
    }
}
