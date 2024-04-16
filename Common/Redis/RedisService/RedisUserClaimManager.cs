using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Redis.Services
{
    /// <summary>
    /// کالایم های دسترسی کاربر
    /// </summary>
    public static class RedisUserClaimManager
    {
        /// <summary>
        /// کلید پیشفرض 
        /// </summary>
        public static readonly string BaseKey = "UserClaims:";

        /// <summary>
        /// مدت زمان ماندگاری در ردیس
        /// </summary>
        public static readonly int ExpMin = 1000;



        /// <summary>
        /// تولید کلید
        /// </summary>
        /// <param name="userId">آیدی مشتری</param>
        /// <returns></returns>
        private static string GetKey(long userId) => BaseKey + "User" + userId;


        /// <summary>
        /// گرفتن کلایم های کاربر
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="userId">آیدی مشتری</param>
        /// <returns>تعداد درخواست کد تخفیف</returns>
        public static async Task<List<string>> GetUserClaims(this IRedisDatabase db, long userId)
        {
            try
            {
                return await db.GetAsync<List<string>>(GetKey(userId));
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// افزودن کلایم های کاربر
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SetUserClaims(this IRedisDatabase db, long userId, List<string> claims, int? expMin = null)
        {
            try
            {
                await db.RemoveUserClaims(userId);
                var isSuccess = await db.AddAsync(GetKey(userId), claims, DateTimeOffset.Now.AddMinutes(expMin ?? ExpMin));
                return isSuccess;
            }
            catch
            {
                return false;
            }
        }




        /// <summary>
        /// حذف کلایم های کاربر
        /// </summary>
        public static async Task<bool> RemoveUserClaims(this IRedisDatabase db, long userId)
        {
            try
            {
                return await db.RemoveAsync(GetKey(userId));
            }
            catch
            {
                return false;
            }
        }


    }
}
