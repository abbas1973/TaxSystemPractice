using Application.Contracts;
using Application.DTOs;
using Application.Filters;
using Application.Utilities;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Utilities;

namespace Application.Features.Web.Invoices
{
    public class InvoiceExcelReportsDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "شماره فاکتور")]
        public string InvoiceNumber { get; set; }

        [Mark]
        [Display(Name = "تاریخ صدور میلای")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "تاریخ صدور")]
        public string InvoiceDateFa => InvoiceDate.ToPersianDateTime().ToShortDateString();

        [Display(Name = "نام خریدار")]
        public string BuyerName { get; set; }

        [Display(Name = "کد ملی خریدار")]
        public string NationalCode { get; set; }

        [Display(Name = "تعداد آیتم ها")]
        public int TotalCount { get; set; }

        [Display(Name = "مبلغ کل")]
        public decimal TotalAmount { get; set; }

        [Mark]
        [Display(Name = "کد روش تسویه")]
        public InvoicePayType PayType { get; set; }

        [Display(Name = "روش تسویه")]
        public string PayTypeFa => PayType.GetEnumDescription();

        [Mark]
        [Display(Name = "کد نوع صورتحساب")]
        public TaxInvoiceType TaxInvoiceType { get; set; }
        [Display(Name = "نوع صورتحساب")]
        public string TaxInvoiceTypeFa => TaxInvoiceType.GetEnumDescription();


        [Mark]
        [Display(Name = "کد الگوی صورتحساب")]
        public TaxInvoicePattern TaxInvoicePattern { get; set; }
        [Display(Name = "الگوی صورتحساب")]
        public string TaxInvoicePatternFa => TaxInvoicePattern.GetEnumDescription();


        [Mark]
        [Display(Name = "کد موضوع سند مالیاتی")]
        public TaxInvoiceSubjectType TaxInvoiceSubject { get; set; }
        [Display(Name = "موضوع سند مالیاتی")]
        public string TaxInvoiceSubjectFa => TaxInvoiceSubject.GetEnumDescription();


        [Mark]
        [Display(Name = "کدوضعیت صورتحساب")]
        public InvoiceSendingStatus SendStatus { get; set; }
        [Display(Name = "وضعیت صورتحساب")]
        public string SendStatusFa => SendStatus.GetEnumDescription();


        [Display(Name = "شماره سریال")]
        public long? SerialNumber { get; set; }

        [Display(Name = "شناسه یکتای مالیاتی")]
        public string TaxId { get; set; }


        [Mark]
        [Display(Name = "تاریخ ارسال میلادی")]
        public DateTime? SendDate { get; set; }
        [Display(Name = "تاریخ ارسال")]
        public string SendDateFa => SendDate.ToPersianDateTime()?.ToString();

        [Display(Name = "وضعیت استعلام")]
        public string TaxStatus { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<Invoice, InvoiceExcelReportsDTO>> Selector
        {
            get
            {
                return model => new InvoiceExcelReportsDTO()
                {
                    Id = model.Id,
                    InvoiceNumber = model.InvoiceNumber,
                    BuyerName = model.BuyerName,
                    NationalCode = model.BuyerNationalCode,
                    InvoiceDate = model.InvoiceDate,
                    TaxId = model.TaxId,
                    TotalCount = model.InvoiceItems.Count(),
                    TotalAmount = model.TotalAmount,
                    PayType = model.PayType,
                    SendDate = model.SendDate,
                    SendStatus = model.SendStatus,
                    SerialNumber = model.SerialNumber,
                    TaxInvoicePattern = model.TaxInvoicePattern,
                    TaxInvoiceSubject = model.TaxInvoiceSubject,
                    TaxInvoiceType = model.TaxInvoiceType,
                    TaxStatus = model.TaxStatus
                };
            }
        }
        #endregion
    }
}
