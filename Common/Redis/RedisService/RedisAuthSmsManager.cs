using StackExchange.Redis.Extensions.Core.Abstractions;
using Utilities;

namespace Redis.Services
{
    /// <summary>
    /// api - تعداد تلاش ها برای ارسال کد احراز هویت 
    /// </summary>
    public static class RedisAuthSmsManager
    {
        /// <summary>
        /// کلید پیشفرض 
        /// </summary>
        public static readonly string BaseKey = "AuthCode:";

        /// <summary>
        /// مدت زمان ماندگاری در ردیس
        /// </summary>
        public static readonly int ExpMin = 2;



        /// <summary>
        /// تولید کلید
        /// </summary>
        /// <param name="mobile">موبایل مشتری</param>
        /// <returns></returns>
        private static string GetKey(string mobile) => BaseKey + mobile;


        /// <summary>
        /// آیا کاربر تا 2 دقیقه قبل درخواست کد احراز هویت داده است؟
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="mobile">تلفن مشتری</param>
        public static async Task<bool> HasAuthSms(this IRedisDatabase db, string mobile)
        {
            try
            {
                return await db.GetAsync<string>(GetKey(mobile)) != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// افزودن درخواست کد احراز هویت به ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="mobile">آیدی مشتری</param>
        /// <param name="expMin">مدت زمان اعتبار</param>
        /// <returns></returns>
        public static async Task<bool> SetAuthSms(this IRedisDatabase db, string mobile, int? expMin = null)
        {
            try
            {
                if (await db.HasAuthSms(mobile))
                    return true;
                var isSuccess = await db.AddAsync(GetKey(mobile), DateTime.Now.ToPersianDateTime().ToString(), DateTimeOffset.Now.AddMinutes(expMin ?? ExpMin));
                if (isSuccess)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }




        /// <summary>
        /// حذف تعداد درخواست کد احراز هویت از ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="mobile">آیدی مشتری</param>
        /// <returns></returns>
        public static async Task<bool> RemoveAuthSms(this IRedisDatabase db, string mobile)
        {
            try
            {
                return await db.RemoveAsync(GetKey(mobile));
            }
            catch
            {
                return false;
            }
        }


    }
}
