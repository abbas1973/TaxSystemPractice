using Application.DTOs;
using Application.Utilities;
using AutoMapper;
using Base.Application.Mapping;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Utilities;

namespace Application.Features.Web.Invoices
{
    public class InvoiceGetByIdDTO : BaseEntityDTO, IMapFrom<Invoice>
    {
        #region Properties
        [Display(Name = "شماره صورتحساب")]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// تاریخ صدور فاکتور
        /// </summary>
        [Display(Name = "تاریخ صدور صورتحساب")]
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDateFa => InvoiceDate.ToPersianDateTime().ToString();


        /// <summary>
        /// وضعیت ارسال به سامانه مالیاتی
        /// </summary>
        [Display(Name = "وضعیت ارسال به سامانه مالیاتی")]
        public InvoiceSendingStatus SendStatus { get; set; }
        public string SendStatusFa => SendStatus.GetEnumDescription();




        #region مبالغ تجمیعی فاکتور
        /// <summary>
        /// مجموع مبلغ کالا/خدمات قبل از اعمال تخفیف
        /// </summary>
        [Display(Name = "مجموع مبلغ کالا/خدمات قبل از اعمال تخفیف")]
        public decimal TotalPriceBeforDiscount { get; set; }

        /// <summary>
        /// مجموع تخفیفات
        /// </summary>
        [Display(Name = "مجموع تخفیفات")]
        public decimal DiscountAmount { get; set; }


        /// <summary>
        /// مجموع مبلغ کالا/خدمات بعد از اعمال تخفیف
        /// <para>
        /// TotalPriceAfterDiscount = TotalPriceBeforDiscount - DiscountAmount
        /// </para>
        /// </summary>
        [Display(Name = "مجموع مبلغ کالا/خدمات بعد از اعمال تخفیف")]
        public decimal TotalPriceAfterDiscount { get; set; }


        /// <summary>
        /// مجموع مالیات بر ارزش افزوده
        /// </summary>
        [Display(Name = "مجموع مالیات بر ارزش افزوده")]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// مجموع سایر مالیات، عوارض و وجوه قانونی
        /// </summary>
        [Display(Name = "مجموع سایر مالیات")]
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
        [Display(Name = "مجموع صورتحساب")]
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
        [Display(Name = "مبلغ پرداختی نقدی")]
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
        [Display(Name = "مبلغ پرداختی نسیه")]
        public decimal CreditAmount { get; set; }
        #endregion



        #region اطلاعات خریدار
        /// <summary>
        /// نام خریدار
        /// </summary>
        [Display(Name = "نام خریدار")]
        public string BuyerName { get; set; }


        /// <summary>
        /// کد ملی  خریدار حقیقی و شناسه ملی خریدار حقوقی 
        /// <para>
        /// اجباری
        /// </para>
        /// </summary>
        [Display(Name = "شناسه ملی  خریدار")]
        public string BuyerNationalCode { get; set; }

        /// <summary>
        /// شماره اقتصادی خریدار
        /// <para>
        /// اختیاری
        /// </para>
        /// </summary>
        [Display(Name = "شماره اقتصادی خریدار")]
        public string BuyerEconomicCode { get; set; }

        /// <summary>
        /// موبایل خریدار
        /// </summary>
        [Display(Name = "موبایل خریدار")]
        public string BuyerMobile { get; set; }

        /// <summary>
        /// آدرس خریدار
        /// </summary>
        [Display(Name = "آدرس خریدار")]
        public string BuyerAddress { get; set; }

        /// <summary>
        /// کد پستی خریدار
        /// </summary>
        [Display(Name = "کد پستی خریدار")]
        public string BuyerPostalCode { get; set; }

        /// <summary>
        /// تلفن خریدار
        /// </summary>
        [Display(Name = "تلفن خریدار")]
        public string BuyerPhone { get; set; }

        /// <summary>
        /// حقیقی یا حقوقی بودن خریدار
        /// <para>
        /// 1 => حقیقی
        /// 2 => حقوقی
        /// </para>
        /// </summary>
        [Display(Name = "حقیقی یا حقوقی بودن خریدار")]
        public BuyerType BuyerIsRealOrLegal { get; set; }
        public string BuyerIsRealOrLegalFa => BuyerIsRealOrLegal.GetEnumDescription();
        #endregion


        /// <summary>
        /// توضیحات صورتحساب
        /// </summary>
        [Display(Name = "توضیحات صورتحساب")]
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
        [Display(Name = "روش تسویه فاکتور")]
        public InvoicePayType PayType { get; set; } = InvoicePayType.Cash;
        public string PayTypeFa => PayType.GetEnumDescription();


        /// <summary>
        /// نوع صورتحساب برای سامانه مالیاتی
        /// <para>
        /// 1 => نوع اول - همه الگوها مجاز است
        /// 2 => نوع دوم - الگوی اول و سوم مجاز است
        /// </para>
        /// </summary>
        [Display(Name = "نوع صورتحساب برای سامانه مالیاتی")]
        public TaxInvoiceType TaxInvoiceType { get; set; }
        public string TaxInvoiceTypeFa => TaxInvoiceType.GetEnumDescription();

        /// <summary>
        /// الگوی  مالیاتی صورتحساب
        /// <para>
        /// با توجه به نوع صورتحساب مقادیر خاصی مجاز است
        /// </para>
        /// </summary>
        [Display(Name = "الگوی  مالیاتی صورتحساب")]
        public TaxInvoicePattern TaxInvoicePattern { get; set; }
        public string TaxInvoicePatternFa => TaxInvoicePattern.GetEnumDescription();


        [Display(Name = "شناسه قرارداد")]
        public string ContractId { get; set; }



        /// <summary>
        /// موضوع سند مالیاتی
        /// </summary>
        [Display(Name = "موضوع سند مالیاتی")]
        public TaxInvoiceSubjectType TaxInvoiceSubject { get; set; }
        public string TaxInvoiceSubjectFa => TaxInvoiceSubject.GetEnumDescription();
        #endregion


        #region فیلد های دریافتی از سامانه مالیاتی
        #region قبل از ارسال صورتحساب
        /// <summary>
        /// شماره سریال برای فاکتور ها به ترتیب ارسال به سامانه مالیاتی پر میشود
        /// </summary>
        [Display(Name = "شماره سریال")]
        public long? SerialNumber { get; set; }

        /// <summary>
        /// شماره منحصر به فرد مالیاتی
        /// پیش از ارسال صورتحساب توسط سامانه مالیاتی ایجاد میشود.
        /// <para>
        /// شمارهای شامل بیست و دو کاراکتر و دارای چهار بخش است: شناسه حافظه مالیاتی )شش کاراکتر(، تاریخ صورتحساب )پنج
        /// کاراکتر(، سریال صورتحساب)ده کاراکتر(و ارقام کنترلی )یک کاراکتر(که به هر صورتحساب اختصاص داده میشود.
        /// </para>
        /// </summary>
        [Display(Name = "شماره منحصر به فرد مالیاتی")]
        public string TaxId { get; set; }
        #endregion


        #region بعد از ارسال صورتحساب
        #region اطلاعاتی که بلافاصله بعد از ارسال صورتحساب دریافت میشود
        [Display(Name = "تاریخ ارسال به سامانه مودیان")]
        public DateTime? SendDate { get; set; }
        public string SendDateFa => SendDate.ToPersianDateTime()?.ToString();

        [Display(Name = "uid")]
        public string TaxUid { get; set; }

        [Display(Name = "شماره رفرنس")]
        public string TaxRefNumber { get; set; }

        [Display(Name = "کد خطا")]
        public string TaxErrorCode { get; set; }

        [Display(Name = "توضیحات خطا")]
        public string TaxErrorDetail { get; set; }
        #endregion


        #region اطلاعاتی که پس از استعلام وضعیت درخواست دریافت میشود
        [Display(Name = "وضعیت استعلام")]
        public string TaxStatus { get; set; }

        [Display(Name = "توضیحات وضعیت استعلام")]
        public string TaxStatusMessage { get; set; }

        [Display(Name = "نوع پکت")]
        public string TaxPacketType { get; set; }

        [Display(Name = "FiscalId")]
        public string TaxFiscalId { get; set; }

        [Display(Name = "اطلاعات استعلام")]
        public string TaxInquiryData { get; set; }
        #endregion
        #endregion
        #endregion


        /// <summary>
        /// آیتم های درون صورتحساب
        /// </summary>
        public List<InvoiceItemDTO> InvoiceItems { get; set; }
        #endregion


        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<Invoice, InvoiceGetByIdDTO>()
           .ForMember(dest => dest.InvoiceItems, opt => opt.MapFrom(src => src.InvoiceItems));
        #endregion
    }



    /// <summary>
    /// مدل آیتم های صورتحساب
    /// </summary>
    public class InvoiceItemDTO : IMapFrom<InvoiceItem>
    {
        #region Properties
        /// <summary>
        /// نام کالا یا خدمت
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// کد کالا یا خدمت
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// تعداد 
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// واحد شمارش
        /// </summary>
        public string CountingUnitName { get; set; }

        /// <summary>
        /// قیمت واحد (قیمت یک آیتم)
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// مبلغ کل بدون تخفیف با احتساب تعداد
        /// <para>
        /// Quantity * UnitPrice
        /// </para>
        /// </summary>
        public decimal TotalPriceBeforDiscount { get; set; }


        /// <summary>
        /// مبلغ تخفیف برای همه تعداد خریداری شده
        /// </summary>
        public decimal DiscountAmount { get; set; }


        /// <summary>
        /// مبلغ کل با اعمال تخفیف
        /// <para>
        /// (Quantity * UnitPrice) - DiscountAmount
        /// </para>
        /// </summary>
        public decimal TotalPriceAfterDiscount { get; set; }


        /// <summary>
        /// درصد مالیات بر ارزش افزوده
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// مبلغ مالیات بر ارزش افزوده برای همه تعداد خریداری شده
        /// </summary>
        public decimal TaxAmount { get; set; }


        /// <summary>
        /// مبلغ سایر مالیات، عوارض و وجوه قانونی
        /// </summary>
        public decimal OtherTaxAmount { get; set; }


        /// <summary>
        /// مبلغ کل با اعمال تخفیف و مالیات
        /// <para>
        /// (Quantity * UnitPrice) - DiscountAmount + TaxAmount + OtherTaxAmount
        /// </para>
        /// </summary>
        public decimal TotalPrice { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<InvoiceItem, InvoiceItemDTO>();
        #endregion
    }


}
