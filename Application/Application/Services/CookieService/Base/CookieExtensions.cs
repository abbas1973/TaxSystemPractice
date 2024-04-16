using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.CookieServices
{
    /// <summary>
    /// خوندن و نوشتن از کوکی
    /// </summary>
    public static class CookieExtensions
    {
        public const string SessionCookieKey = "_session.cookie";
        public const string CsrfCookieKey = "_csrf.cookie";

        private static object sync = new object();



        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTimeAsDay">expiration time</param>  
        public static void Set(this IResponseCookies ResponseCookies, string key, string value, int? expireTimeAsDay = 10, bool HttpOnly = false, DateTime? ExpDate = null)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = ExpDate ?? DateTime.Now.AddDays(expireTimeAsDay ?? 10).Date + new TimeSpan(23, 59, 59);
            option.Path = "/";
            option.HttpOnly = HttpOnly;
            option.IsEssential = true;
            option.SameSite = SameSiteMode.None;
            //option.Secure = true;
            ResponseCookies.Append(key, value, option);
        }



        /// <summary>
        /// قرار دادن مقدار در کوکی
        /// </summary>
        /// <typeparam name="T">نوع داده ای که درون کوکی قرار میگیرد</typeparam>
        /// <param name="ResponseCookies">ریسپانس کوکی</param>
        /// <param name="key">کلید</param>
        /// <param name="value">داده ای که درون کوکی قرار میگیرد</param>
        public static void Set<T>(this IResponseCookies ResponseCookies, string key, T value, int? expireTimeAsDay = 5, bool HttpOnly = false, DateTime? ExpDate = null)
        {
            var st = JsonConvert.SerializeObject(value);
            ResponseCookies.Set(key, st, expireTimeAsDay, HttpOnly, ExpDate);
        }




        /// <summary>
        /// گرفتن مقدا از کوکی
        /// </summary>
        /// <typeparam name="T">نوع داده ای که درون کوکی قرار میگیرد</typeparam>
        /// <param name="RequestCookies">ریکوئست کوکی</param>
        /// <param name="key">کلید</param>
        /// <returns></returns>
        public static T Get<T>(this IRequestCookieCollection RequestCookies, string key)
        {
            string value = RequestCookies.Get(key);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }



        /// <summary>
        /// گرفتن مقدا از کوکی
        /// </summary>
        /// <typeparam name="T">نوع داده ای که درون کوکی قرار میگیرد</typeparam>
        /// <param name="Request">ریکوئست</param>
        /// <param name="key">کلید</param>
        /// <returns></returns>
        public static string Get(this IRequestCookieCollection RequestCookies, string key)
        {
            string value = RequestCookies[key];
            return value;
        }




        /// <summary>
        /// یک داده را از کوکی میخواند.
        /// در صورتی که داده مورد نظر در کوکی وجود نداشته باشد،
        /// از تابعی که در ورودی پاس داده میشود، مقدار مورد نظر را لود میکند و برمیگرداند.
        /// </summary>
        /// <typeparam name="T">نوع خروجی</typeparam>
        /// <param name="HttpContext">کانتکست</param>
        /// <param name="key">کلید</param>
        /// <param name="generator">تابعی که در صورت خالی بودن کوکی، آنرا پر میکند</param>
        /// <returns></returns>
        public static T GetOrStoreCookie<T>(this HttpContext HttpContext, string key, Func<T> generator)
        {
            var value = HttpContext.Request.Cookies.Get<T>(key);
            return HttpContext.GetOrStoreCookie(key, (value == null && generator != null) ? generator() : default(T));
        }





        /// <summary>
        /// یک داده را از کوکی میخواند.
        /// در صورتی که داده مورد نظر در کوکی وجود نداشته باشد،
        /// مقدار پاس داده شده در ورودی را درون کوکی قرار میدهد
        /// </summary>
        /// <typeparam name="T">نوع خروجی</typeparam>
        /// <param name="HttpContext">کانتکست</param>
        /// <param name="key">کلید</param>
        /// <param name="obj">داده ای که درون کوکی قرار میگیرد</param>
        /// <returns></returns>
        public static T GetOrStoreCookie<T>(this HttpContext HttpContext, string key, T obj)
        {
            var RequestCookies = HttpContext.Request.Cookies;
            var ResponseCookies = HttpContext.Response.Cookies;
            T result = RequestCookies.Get<T>(key);

            if (result == null)
            {
                lock (sync)
                {
                    result = RequestCookies.Get<T>(key);
                    if (result == null)
                    {
                        result = obj != null ? obj : default(T);
                        ResponseCookies.Set<T>(key, result);
                    }
                }
            }
            return result;
        }






        public static void RemoveCookie(this HttpContext HttpContext, string Key)
        {
            var RequestCookies = HttpContext.Request.Cookies;
            var ResponseCookies = HttpContext.Response.Cookies;
            if (RequestCookies.Get(Key) != null)
                ResponseCookies.Delete(Key);
        }



        public static bool RemoveSettingCookie(this HttpContext HttpContext)
        {
            try
            {
                HttpContext?.RemoveCookie(SessionCookieKey);
                HttpContext?.RemoveCookie(CsrfCookieKey);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}








   