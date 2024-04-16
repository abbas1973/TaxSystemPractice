using System.ComponentModel;

namespace Domain.Enums
{
    /// <summary>
    /// وضعیت ارسال صورتحساب به سامانه مالیاتی
    /// </summary>
    public enum InvoiceSendingStatus
    {

        /// <summary>
        /// ارسال نشده
        /// </summary>
        [Description("ارسال نشده")]
        NotSent = 1,


        /// <summary>
        /// ارسال شده
        /// </summary>
        [Description("ارسال شده")]
        Sent = 2,



        /// <summary>
        /// استعلام شده
        /// </summary>
        [Description("استعلام شده")]
        Inquired = 3
    }
}
