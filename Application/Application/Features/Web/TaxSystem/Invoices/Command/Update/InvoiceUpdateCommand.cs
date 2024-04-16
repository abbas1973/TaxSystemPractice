using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using Application.SessionServices;
using AutoMapper;
using Base.Application.Mapping;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Invoices
{
    #region Request
    public class InvoiceUpdateCommand
    : BaseEntityDTO, IRequest<BaseResult<long>>, IMapFrom<Invoice>
    {
        #region Constructors
        public InvoiceUpdateCommand()
        {

        }
        #endregion


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
        public List<InvoiceItemUpdateDTO> InvoiceItems { get; set; } = new List<InvoiceItemUpdateDTO>();
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<InvoiceUpdateCommand, Invoice>()
            .ForMember(dest => dest.InvoiceItems, opt => opt.MapFrom(src => src.InvoiceItems))
            .ForMember(dest => dest.SendStatus, opt => opt.MapFrom(src => InvoiceSendingStatus.NotSent))
            .ForMember(dest => dest.TotalPriceBeforDiscount, opt => opt.MapFrom(src => src.InvoiceItems.Sum(x => x.Quantity * x.UnitPrice)))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.InvoiceItems.Sum(x => x.DiscountAmount)))
            .ForMember(dest => dest.TotalPriceAfterDiscount, opt => opt.MapFrom(src => src.InvoiceItems.Sum(x => (x.Quantity * x.UnitPrice) - x.DiscountAmount)))
            .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.InvoiceItems.Sum(x => x.TaxAmount)))
            .ForMember(dest => dest.OtherTaxAmount, opt => opt.MapFrom(src => src.InvoiceItems.Sum(x => x.OtherTaxAmount)))
            .ForMember(dest => dest.CreditAmount, opt => opt.MapFrom(src => src.InvoiceItems.Sum(x => (x.Quantity * x.UnitPrice) - x.DiscountAmount + x.TaxAmount + x.OtherTaxAmount) - src.CashAmount))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.InvoiceItems.Sum(x => (x.Quantity * x.UnitPrice) - x.DiscountAmount + x.TaxAmount + x.OtherTaxAmount)));
        #endregion
    }


    #region مدل آیتم های صورتحساب برای ایجاد
    public record class InvoiceItemUpdateDTO : IMapFrom<InvoiceItem>
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
            profile.CreateMap<InvoiceItemUpdateDTO, InvoiceItem>()
            .ForMember(dest => dest.TotalPriceBeforDiscount, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice))
            .ForMember(dest => dest.TotalPriceAfterDiscount, opt => opt.MapFrom(src => (src.Quantity * src.UnitPrice) - src.DiscountAmount))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => (src.Quantity * src.UnitPrice) - src.DiscountAmount + src.TaxAmount + src.OtherTaxAmount));
        #endregion
    }
    #endregion

    #endregion



    #region Handler
    public class InvoiceUpdateCommandHandler : IRequestHandler<InvoiceUpdateCommand, BaseResult<long>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        public InvoiceUpdateCommandHandler(
            IUnitOfWork uow, 
            IMapper mapper, 
            IHttpContextAccessor accessor)
        {
            _uow = uow;
            _mapper = mapper;
            _accessor = accessor;
        }


        public async Task<BaseResult<long>> Handle(InvoiceUpdateCommand request, CancellationToken cancellationToken)
        {
            #region گرفتن شرکت
            var user = _accessor.HttpContext.Session.GetUser();
            var companyId = user.CompanyId;
            if (companyId == null)
                companyId = await _uow.Companies.GetOneDTOAsync<long?>(x => x.Id, orderBy: x => x.OrderBy(z => z.Id));
            if (companyId == null || companyId == 0)
                throw new NotFoundException("ابتدا شرکت مورد نظر را ایجاد کنید!");
            #endregion

            var invoice = _uow.Invoices.GetById(request.Id);
            var Invoice = _mapper.Map(request, invoice);
            Invoice.CompanyId = (long)companyId;
            await _uow.Invoices.AddAsync(Invoice);
            await _uow.CommitAsync();
            return new BaseResult<long>(Invoice.Id);
        }
    }

    #endregion
}
