using System.ComponentModel;

namespace Application.Filters
{
    /// <summary>
    /// انواع سطوح احراز هویت برای کنترلر و اکشن ها
    /// </summary>
    public enum AuthorizeType
    {
        /// <summary>
        /// کاربر لاگین کرده باشد کافی است
        /// </summary>
        [Description("نیاز به لاگین")]
        NeedAuthentication = 0,

        /// <summary>
        /// کاربر باید لاگین کرده باشد و به منو مورد نظر دسترسی داشته باشد.
        /// </summary>
        [Description("نیاز به دسترسی")]
        NeedPermission = 1


    }



    /// <summary>
    /// سطح دسترسی. برای اعضا است یا کاربران
    /// </summary>
    public enum AuthLevel
    {
        // کاربر
        User = 10,


    }
}
