using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Api.Services.RedisService
{
    /// <summary>
    /// api - تعداد تلاش ها برای بررسی کد تایید 
    /// </summary>
    public static class RedisVerifyCodeCountManager
    {
        /// <summary>
        /// کلید پیشفرض 
        /// </summary>
        public static readonly string BaseKey = "VerifyCode:";

        /// <summary>
        /// مدت زمان ماندگاری در ردیس
        /// </summary>
        public static readonly int ExpMin = 30;



        /// <summary>
        /// تولید کلید
        /// </summary>
        /// <param name="mobile">موبایل مشتری</param>
        /// <returns></returns>
        private static string GetKey(string mobile) => BaseKey + "User" + mobile;


        /// <summary>
        /// گرفتن تعداد درخواست کد تخفیف از ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="mobile">موبایل مشتری</param>
        /// <returns>تعداد درخواست بررسی کد احراز هویت</returns>
        public static async Task<int> GetVerifyCodeCount(this IRedisDatabase db, string mobile)
        {
            try
            {
                return await db.GetAsync<int>(GetKey(mobile));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// افزودن تعداد درخواست کد تخفیف به ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="Cats">دسته بندی محصولات</param>
        /// <param name="mobile">موبایل مشتری</param>
        /// <param name="expMin">مدت زمان اعتبار</param>
        /// <returns></returns>
        public static async Task<int> SetVerifyCodeCount(this IRedisDatabase db, string mobile, int? expMin = null)
        {
            try
            {
                var count = await db.GetVerifyCodeCount(mobile);
                if (count > 0)
                    await db.RemoveVerifyCodeCount(mobile);
                count++;
                var isSuccess = await db.AddAsync(GetKey(mobile), count, DateTimeOffset.Now.AddMinutes(expMin ?? ExpMin));
                if (isSuccess)
                    return count;
                return 0;
            }
            catch
            {
                return 0;
            }
        }




        /// <summary>
        /// حذف تعداد درخواست کد تخفیف از ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="mobile">موبایل مشتری</param>
        /// <returns></returns>
        public static async Task<bool> RemoveVerifyCodeCount(this IRedisDatabase db, string mobile)
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
