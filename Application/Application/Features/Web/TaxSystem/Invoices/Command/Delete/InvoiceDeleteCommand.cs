using MediatR;
using Domain.Entities;
using Application.Repositories;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Base;

namespace Application.Features.Web.Invoices
{
    #region Request
    public class InvoiceDeleteCommand
        :DeleteCommand<Invoice>
    {
        #region Constructors
        public InvoiceDeleteCommand() : base() { }
        public InvoiceDeleteCommand(long id) : base(id) { }
        #endregion        
    }
    #endregion



    #region Handler
    public class InvoiceDeleteQueryHandler : IRequestHandler<InvoiceDeleteCommand, BaseResult>
    {
        private readonly IUnitOfWork _mainUow;
        public InvoiceDeleteQueryHandler(
            IUnitOfWork mainUow)
        {
            _mainUow = mainUow;
        }


        public async Task<BaseResult> Handle(InvoiceDeleteCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _mainUow.Invoices.GetByIdAsync(request.Id);
            if (invoice == null)
                throw new NotFoundException("صورتحساب یافت نشد!");
            if(invoice.SendStatus != Domain.Enums.InvoiceSendingStatus.NotSent)
                throw new NotFoundException("صورتحساب به سامانه مودیان مالیاتی ارسال شده است و امکان حذف وجود ندارد!");
        
            var deletedInvoiceItemCount = await _mainUow.InvoiceItems.ExecuteDeleteAsync(x => x.InvoiceId == request.Id);
            var deletedInvoices = await _mainUow.Invoices.ExecuteDeleteAsync(x => x.Id == request.Id);
            if (deletedInvoices == 0)
                throw new NotFoundException("صورتحساب مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
