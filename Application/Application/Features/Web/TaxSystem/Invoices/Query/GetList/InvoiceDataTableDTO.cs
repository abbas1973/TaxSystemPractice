using Application.Contracts;
using Application.DTOs;
using Application.Utilities;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Utilities;

namespace Application.Features.Web.Invoices
{
    public class InvoiceDataTableDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "شماره فاکتور")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDateFa => InvoiceDate.ToPersianDateTime().ToShortDateString();

        [Display(Name = "نام خریدار")]
        public string BuyerName { get; set; }

        [Display(Name = "کد ملی خریدار")]
        public string NationalCode { get; set; }

        [Display(Name = "تعداد آیتم ها")]
        public int TotalCount { get; set; }

        [Display(Name = "مبلغ کل")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "روش تسویه")]
        public InvoicePayType PayType { get; set; }
        public string PayTypeFa => PayType.GetEnumDescription();

        [Display(Name = "نوع صورتحساب")]
        public TaxInvoiceType TaxInvoiceType { get; set; }
        public string TaxInvoiceTypeFa => TaxInvoiceType.GetEnumDescription();


        [Display(Name = "الگوی صورتحساب")]
        public TaxInvoicePattern TaxInvoicePattern { get; set; }
        public string TaxInvoicePatternFa => TaxInvoicePattern.GetEnumDescription();


        [Display(Name = "موضوع سند مالیاتی")]
        public TaxInvoiceSubjectType TaxInvoiceSubject { get; set; }
        public string TaxInvoiceSubjectFa => TaxInvoiceSubject.GetEnumDescription();


        [Display(Name = "وضعیت صورتحساب")]
        public InvoiceSendingStatus SendStatus { get; set; }
        public string SendStatusFa => SendStatus.GetEnumDescription();


        [Display(Name = "شماره سریال")]
        public long? SerialNumber { get; set; }

        [Display(Name = "شناسه یکتای مالیاتی")]
        public string TaxId { get; set; }


        [Display(Name = "تاریخ ارسال")]
        public DateTime? SendDate { get; set; }
        public string SendDateFa => SendDate.ToPersianDateTime()?.ToString();

        [Display(Name = "وضعیت استعلام")]
        public string TaxStatus { get; set; }

        /// <summary>
        /// قابل حذف است؟
        /// <para>
        /// اگر به سامانه مودیان ارسال نشده باشد قابل حذف است.
        /// </para>
        /// </summary>
        public bool IsDeleteable => SendStatus == InvoiceSendingStatus.NotSent;

        /// <summary>
        /// قابل ارسال به سامانه مالیاتی است؟
        /// <para>
        /// ارسال نشده باشد
        /// یا ارسال شده باشد و نتیجه استعلام خطا باشد
        /// </para>
        /// </summary>
        public bool IsSendable => 
            (SendStatus == InvoiceSendingStatus.NotSent) // ارسال نشده باشد
            || (SendStatus == InvoiceSendingStatus.Inquired // استعلام شده باشد
                && TaxStatus == nameof(TaxInquiryStatusMessage.FAILED)); // صورتحساب دارای خطا باشد
        
        
        public bool IsInquiryable => 
            (SendStatus == InvoiceSendingStatus.Sent) // ارسال شده باشد 
            || (SendStatus == InvoiceSendingStatus.Inquired // استعلام شده باشد
                && (TaxStatus == nameof(TaxInquiryStatusMessage.IN_PROGRESS) // صورتحساب در حال بررسی باشد
                    || TaxStatus == nameof(TaxInquiryStatusMessage.TIMEOUT))); // بررسی صورتحساب زیاد طول کشیده باشد
        #endregion


        #region سلکتور
        public static Expression<Func<Invoice, InvoiceDataTableDTO>> Selector
        {
            get
            {
                return model => new InvoiceDataTableDTO()
                {
                    Id = model.Id,
                    InvoiceNumber = model.InvoiceNumber,
                    BuyerName = model.BuyerName,
                    NationalCode = model.BuyerNationalCode,
                    InvoiceDate = model.InvoiceDate,
                    TaxId = model.TaxId,
                    TotalCount = model.InvoiceItems.AsQueryable().Count(),
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
