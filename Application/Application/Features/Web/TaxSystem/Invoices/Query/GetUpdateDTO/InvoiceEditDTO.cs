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
    public class InvoiceEditDTO : BaseEntityDTO, IMapFrom<Invoice>
    {

        #region Properties
        /// <summary>
        /// شماره فاکتور در سیستم شرکت
        /// </summary>
        [Display(Name = "شماره فاکتور")]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// تاریخ صدور فاکتور
        /// </summary>
        [Display(Name = "تاریخ صدور فاکتور")]
        public DateTime InvoiceDate { get; set; }


        #region مبالغ تجمیعی فاکتور

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
        public long CashAmount { get; set; }
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
        [Display(Name = "کد/شناسه ملی خریدار")]
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
        #endregion


        /// <summary>
        /// توضیحات صورتحساب
        /// </summary>
        [Display(Name = "توضیحات صورتحساب")]
        public string Description { get; set; }


        #region اطلاعات صورتحساب که برای سامانه مالیاتی مورد نیاز است
        /// <summary>
        /// روش تسویه صورتحساب
        /// <para>
        /// 1 => نقد
        /// 2 => نسیه
        /// 3 => نقد/نسیه
        /// </para>
        /// </summary>
        [Display(Name = "روش تسویه صورتحساب")]
        public InvoicePayType PayType { get; set; }


        /// <summary>
        /// نوع صورتحساب برای سامانه مالیاتی
        /// <para>
        /// 1 => نوع اول - همه الگوها مجاز است
        /// 2 => نوع دوم - الگوی اول و سوم مجاز است
        /// </para>
        /// </summary>
        [Display(Name = "نوع صورتحساب")]
        public TaxInvoiceType TaxInvoiceType { get; set; }

        /// <summary>
        /// الگوی  مالیاتی صورتحساب
        /// <para>
        /// با توجه به نوع صورتحساب مقادیر خاصی مجاز است
        /// </para>
        /// </summary>
        [Display(Name = "الگوی  مالیاتی صورتحساب")]
        public TaxInvoicePattern TaxInvoicePattern { get; set; }

        /// <summary>
        /// شناسه یکتای ثبت قرارداد فروشنده
        /// <para>
        /// برای صورتحساب های نوع اول با الگوی 4 یا همان قرارداد پیمانکاری
        /// </para>
        /// </summary>
        [Display(Name = "شناسه قرارداد")]
        public string ContractId { get; set; }

        /// <summary>
        /// موضوع سند مالیاتی
        /// </summary>
        [Display(Name = "موضوع سند مالیاتی")]
        public TaxInvoiceSubjectType TaxInvoiceSubject { get; set; }
        #endregion


        [Display(Name = "آیتم های صورتحساب")]
        public List<InvoiceItemEditDTO> InvoiceItems { get; set; } = new List<InvoiceItemEditDTO>();
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<Invoice, InvoiceItemEditDTO>(); 
        #endregion
    }



    #region مدل آیتم های صورتحساب برای ایجاد
    public record class InvoiceItemEditDTO : IMapFrom<InvoiceItem>
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
        /// قیمت واحد (قیمت یک آیتم)
        /// </summary>
        public long UnitPrice { get; set; }

        /// <summary>
        /// واحد شمارش
        /// </summary>
        public string CountingUnitName { get; set; }


        /// <summary>
        /// مبلغ تخفیف برای همه تعداد خریداری شده
        /// </summary>
        public long DiscountAmount { get; set; }


        /// <summary>
        /// درصد مالیات بر ارزش افزوده
        /// </summary>
        public float TaxRate { get; set; }

        /// <summary>
        /// مبلغ مالیات بر ارزش افزوده برای همه تعداد خریداری شده
        /// </summary>
        public long TaxAmount { get; set; }


        /// <summary>
        /// مبلغ سایر مالیات، عوارض و وجوه قانونی
        /// </summary>
        public long OtherTaxAmount { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<InvoiceItem, InvoiceItemEditDTO>();
        #endregion
    }
    #endregion



}
