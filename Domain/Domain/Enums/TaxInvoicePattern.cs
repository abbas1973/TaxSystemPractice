using System.ComponentModel;

namespace Domain.Enums
{
    /// <summary>
    /// الگوهای صورتحساب الکترونیکی
    /// </summary>
    public enum TaxInvoicePattern
    {
        /// <summary>
        /// الگوی اول - فروش
        /// </summary>
        [Description("الگوی اول - فروش")]
        Pattern1 = 1,


        /// <summary>
        /// الگوی دوم - فروش ارز
        /// </summary>
        [Description("الگوی دوم - فروش ارز")]
        Pattern2 = 2,


        /// <summary>
        /// الگوی سوم - صورت حساب طلا، جواهر و پلاتین
        /// </summary>
        [Description("الگوی سوم - صورت حساب طلا، جواهر و پلاتین")]
        Pattern3 = 3,


        /// <summary>
        /// الگوی چهارم - قرارداد پیمانکاری
        /// </summary>
        [Description("الگوی چهارم - قرارداد پیمانکاری")]
        Pattern4 = 4,


        /// <summary>
        /// الگوی پنجم - قبوض خدماتی
        /// </summary>
        [Description("الگوی پنجم - قبوض خدماتی")]
        Pattern5 = 5,


        /// <summary>
        /// الگوی ششم - بلیط هواپیما
        /// </summary>
        [Description("الگوی ششم - بلیط هواپیما")]
        Pattern6 = 6,


        /// <summary>
        /// الگوی هفتم - صادرات
        /// </summary>
        [Description("الگوی هفتم - صادرات")]
        Pattern7 = 7


    }
}
