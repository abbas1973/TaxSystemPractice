using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Utilities;

namespace Application.Features.Web.Invoices
{
    #region Request
    public class InvoiceGetListQuery
    : IRequest<DataTableResponseDTO<InvoiceDataTableDTO>>
    {
        #region Constructors
        public InvoiceGetListQuery()
        {
        }
        #endregion


        #region Properties
        [Display(Name = "شماره فاکتور")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "شماره مالیاتی فاکتور")]
        public string TaxId { get; set; }

        [Display(Name = "شماره سریال")]
        public long? SerialNumber { get; set; }

        [Display(Name = "کد ملی خریدار")]
        public string BuyerNationalCode { get; set; }

        [Display(Name = "نام خریدار")]
        public string BuyerName { get; set; }

        [Display(Name = "تاریخ فاکتور از")]
        public DateTime? InvoiceDateFrom { get; set; }

        [Display(Name = "تاریخ فاکتور تا")]
        public DateTime? InvoiceDateTo { get; set; }

        [Display(Name = "روش تسویه")]
        public InvoicePayType? PayType { get; set; }

        [Display(Name = "نوع صورتحساب")]
        public TaxInvoiceType? TaxInvoiceType { get; set; }

        [Display(Name = "الگوی صورتحساب")]
        public TaxInvoicePattern? TaxInvoicePattern { get; set; }

        [Display(Name = "موضوع سند مالیاتی")]
        public TaxInvoiceSubjectType? TaxInvoiceSubject { get; set; }

        [Display(Name = "وضعیت صورتحساب")]
        public InvoiceSendingStatus? SendStatus { get; set; }

        [Display(Name = "تاریخ ارسال از")]
        public DateTime? SendDateFrom { get; set; }

        [Display(Name = "تاریخ ارسال تا")]
        public DateTime? SendDateTo { get; set; }
        #endregion


        #region توابع
        public Expression<Func<Invoice, bool>> GetFilter(string dataTableSearch)
        {
            var filter = PredicateBuilder.New<Invoice>(true);

            if (!string.IsNullOrWhiteSpace(InvoiceNumber))
                filter.And(x => x.InvoiceNumber.Contains(InvoiceNumber));

            if (!string.IsNullOrWhiteSpace(TaxId))
                filter.And(x => x.TaxId == TaxId);

            if (!string.IsNullOrWhiteSpace(BuyerNationalCode))
                filter.And(x => x.BuyerNationalCode == BuyerNationalCode);

            if (!string.IsNullOrWhiteSpace(BuyerName))
                filter.And(x => x.BuyerName.Contains(BuyerName));

            if(PayType != null)
                filter.And(x => x.PayType == PayType);

            if(TaxInvoiceType != null)
                filter.And(x => x.TaxInvoiceType == TaxInvoiceType);

            if(TaxInvoicePattern != null)
                filter.And(x => x.TaxInvoicePattern == TaxInvoicePattern);

            if(TaxInvoiceSubject != null)
                filter.And(x => x.TaxInvoiceSubject == TaxInvoiceSubject);

            if(SendStatus != null)
                filter.And(x => x.SendStatus == SendStatus);

            if(SerialNumber != null)
                filter.And(x => x.SerialNumber == SerialNumber);

            if (InvoiceDateFrom != null)
            {
                InvoiceDateFrom = InvoiceDateFrom + new TimeSpan(0, 0, 0);
                filter.And(x => x.InvoiceDate >= InvoiceDateFrom);
            }

            if (InvoiceDateTo != null)
            {
                InvoiceDateTo = InvoiceDateTo + new TimeSpan(23, 59, 59);
                filter.And(x => x.InvoiceDate <= InvoiceDateTo);
            }

            if (SendDateFrom != null)
            {
                SendDateFrom = SendDateFrom + new TimeSpan(0, 0, 0);
                filter.And(x => x.SendDate >= SendDateFrom);
            }

            if (SendDateTo != null)
            {
                SendDateTo = SendDateTo + new TimeSpan(23, 59, 59);
                filter.And(x => x.SendDate <= SendDateTo);
            }
            return filter;
        }
        #endregion

    }
    #endregion



    #region Handler
    public class InvoiceGetListQueryHandler : IRequestHandler<InvoiceGetListQuery, DataTableResponseDTO<InvoiceDataTableDTO>>
    {
        private readonly IDataTableManager _dataTableManager;
        private readonly IUnitOfWork _uow;
        public InvoiceGetListQueryHandler(IUnitOfWork uow, IDataTableManager dataTableManager)
        {
            _uow = uow;
            _dataTableManager = dataTableManager;
        }


        public async Task<DataTableResponseDTO<InvoiceDataTableDTO>> Handle(InvoiceGetListQuery request, CancellationToken cancellationToken)
        {
            var model = new DataTableResponseDTO<InvoiceDataTableDTO>();
            var searchData = _dataTableManager.GetSearchModel();
            var sortCol = $"{searchData.sortColumnName} {searchData.sortDirection}";
            var filter = request.GetFilter(searchData.searchValue);

            model.draw = searchData.draw;
            model.recordsTotal = await _uow.Invoices.CountAsync();
            model.recordsFiltered= await _uow.Invoices.CountAsync(filter);
            model.data = await _uow.Invoices.GetDTOAsync(
                InvoiceDataTableDTO.Selector,
                filter,
                x => x.OrderBy(sortCol),
                searchData.start,
                searchData.length
                );

            return model;
        }
    }

    #endregion
}
