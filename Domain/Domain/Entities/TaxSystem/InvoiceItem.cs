namespace Domain.Entities
{
    public class InvoiceItem : BaseEntity
    {
        #region Properties
        /// <summary>
        /// آیدی در سیستم خود شرکت
        /// </summary>
        public int? InvoiceItemId { get; set; }


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
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// واحد شمارش
        /// </summary>
        public string CountingUnitName { get; set; }


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


        #region Relations
        public long InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        #endregion
    }
}
