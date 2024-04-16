using Domain.Enums;
using System.ComponentModel;

namespace Domain.Entities
{
    [Description("صورتحساب")]
    public class Invoice : BaseEntity
    {
        #region Properties
        /// <summary>
        /// شماره فاکتور در سیستم شرکت
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// نوع بارکد
        /// <para>
        /// 1 => GS1
        /// 2 => Irancode 
        /// </para>
        /// </summary>
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// تاریخ صدور فاکتور
        /// </summary>
        public DateTime InvoiceDate { get; set; }


        /// <summary>
        /// وضعیت ارسال به سامانه مالیاتی
        /// </summary>
        public InvoiceSendingStatus SendStatus { get; set; } = InvoiceSendingStatus.NotSent;



        #region مبالغ تجمیعی فاکتور
        /// <summary>
        /// مجموع مبلغ کالا/خدمات قبل از اعمال تخفیف
        /// </summary>
        public decimal TotalPriceBeforDiscount { get; set; }

        /// <summary>
        /// مجموع تخفیفات
        /// </summary>
        public decimal DiscountAmount { get; set; }


        /// <summary>
        /// مجموع مبلغ کالا/خدمات بعد از اعمال تخفیف
        /// <para>
        /// TotalPriceAfterDiscount = TotalPriceBeforDiscount - DiscountAmount
        /// </para>
        /// </summary>
        public decimal TotalPriceAfterDiscount { get; set; }


        /// <summary>
        /// مجموع مالیات بر ارزش افزوده
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// مجموع سایر مالیات، عوارض و وجوه قانونی
        /// </summary>
        public decimal OtherTaxAmount { get; set; }


        /// <summary>
        /// مجموع صورتحساب
        /// <para>
        /// TotalAmount = TotalPriceAfterDiscount + TaxAmount + OtherTaxAmount
        /// </para>
        /// <para>
        /// TotalAmount = CashAmount + CreditAmount + TaxAmount + OtherTaxAmount
        /// </para>
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// مبلغ پرداختی نقدی
        /// <para>
        /// CashAmount = TotalAmount - CreditAmount - TaxAmount - OtherTaxAmount
        /// </para>
        /// <para>
        /// CashAmount = TotalPriceAfterDiscount - CreditAmount
        /// </para>
        /// </summary>
        public decimal CashAmount { get; set; }

        /// <summary>
        /// مبلغ پرداختی نسیه
        /// <para>
        /// CreditAmount = TotalAmount - CashAmount - TaxAmount - OtherTaxAmount
        /// </para>
        /// <para>
        /// CreditAmount = TotalPriceAfterDiscount - CashAmount
        /// </para>
        /// </summary>
        public decimal CreditAmount { get; set; }
        #endregion



        #region اطلاعات خریدار
        /// <summary>
        /// نام خریدار
        /// </summary>
        public string BuyerName { get; set; }


        /// <summary>
        /// کد ملی  خریدار حقیقی و شناسه ملی خریدار حقوقی 
        /// <para>
        /// اجباری
        /// </para>
        /// </summary>
        public string BuyerNationalCode { get; set; }

        /// <summary>
        /// شماره اقتصادی خریدار
        /// <para>
        /// اختیاری
        /// </para>
        /// </summary>
        public string BuyerEconomicCode { get; set; }

        /// <summary>
        /// موبایل خریدار
        /// </summary>
        public string BuyerMobile { get; set; }

        /// <summary>
        /// آدرس خریدار
        /// </summary>
        public string BuyerAddress { get; set; }

        /// <summary>
        /// کد پستی خریدار
        /// </summary>
        public string BuyerPostalCode { get; set; }

        /// <summary>
        /// تلفن خریدار
        /// </summary>
        public string BuyerPhone { get; set; }

        /// <summary>
        /// حقیقی یا حقوقی بودن خریدار
        /// <para>
        /// 1 => حقیقی
        /// 2 => حقوقی
        /// </para>
        /// </summary>
        public BuyerType BuyerIsRealOrLegal { get; set; } 
        #endregion


        /// <summary>
        /// توضیحات صورتحساب
        /// </summary>
        public string Description { get; set; }


        #region اطلاعات صورتحساب که برای سامانه مالیاتی مورد نیاز است
        /// <summary>
        /// روش تسویه فاکتور
        /// <para>
        /// 1 => نقد
        /// 2 => نسیه
        /// 3 => نقد/نسیه
        /// </para>
        /// </summary>
        public InvoicePayType PayType { get; set; } = InvoicePayType.Cash;


        /// <summary>
        /// نوع صورتحساب برای سامانه مالیاتی
        /// <para>
        /// 1 => نوع اول - همه الگوها مجاز است
        /// 2 => نوع دوم - الگوی اول و سوم مجاز است
        /// </para>
        /// </summary>
        public TaxInvoiceType TaxInvoiceType { get; set; } = TaxInvoiceType.Type1;

        /// <summary>
        /// الگوی  مالیاتی صورتحساب
        /// <para>
        /// با توجه به نوع صورتحساب مقادیر خاصی مجاز است
        /// </para>
        /// </summary>
        public TaxInvoicePattern TaxInvoicePattern { get; set; } = TaxInvoicePattern.Pattern1;


        /// <summary>
        /// موضوع سند مالیاتی
        /// </summary>
        public TaxInvoiceSubjectType TaxInvoiceSubject { get; set; } = TaxInvoiceSubjectType.Main;

        /// <summary>
        /// شناسه یکتای ثبت قرارداد فروشنده
        /// <para>
        /// برای صورتحساب های نوع اول با الگوی 4 یا همان قرارداد پیمانکاری
        /// </para>
        /// </summary>
        public string ContractId { get; set; }
        #endregion


        #region فیلد های دریافتی از سامانه مالیاتی
        #region قبل از ارسال صورتحساب
        /// <summary>
        /// شماره سریال برای فاکتور ها به ترتیب ارسال به سامانه مالیاتی پر میشود
        /// </summary>
        public long? SerialNumber { get; set; }

        /// <summary>
        /// شماره منحصر به فرد مالیاتی
        /// پیش از ارسال صورتحساب توسط سامانه مالیاتی ایجاد میشود.
        /// <para>
        /// شمارهای شامل بیست و دو کاراکتر و دارای چهار بخش است: شناسه حافظه مالیاتی )شش کاراکتر(، تاریخ صورتحساب )پنج
        /// کاراکتر(، سریال صورتحساب)ده کاراکتر(و ارقام کنترلی )یک کاراکتر(که به هر صورتحساب اختصاص داده میشود.
        /// </para>
        /// </summary>
        public string TaxId { get; set; }
        #endregion


        #region بعد از ارسال صورتحساب
        #region اطلاعاتی که بلافاصله بعد از ارسال صورتحساب دریافت میشود
        public DateTime? SendDate { get; set; }
        public string TaxUid { get; set; }
        public string TaxRefNumber { get; set; }
        public string TaxErrorCode { get; set; }
        public string TaxErrorDetail { get; set; }
        #endregion


        #region اطلاعاتی که پس از استعلام وضعیت درخواست دریافت میشود
        public string TaxStatus { get; set; }
        public string TaxStatusMessage { get; set; }
        public string TaxPacketType { get; set; }
        public string TaxFiscalId { get; set; } 
        public string TaxInquiryData { get; set; } 
        #endregion
        #endregion
        #endregion
        #endregion


        #region Relations
        public long CompanyId { get; set; }
        public Company Company { get; set; }
        #endregion


        #region Navigation Properties
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
        #endregion
    }
}
