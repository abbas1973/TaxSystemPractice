using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Redis.Services
{
    /// <summary>
    /// اطلاعات دسترسی اعضا و کاربران
    /// </summary>
    public static class RedisPermissionManager
    {
        /// <summary>
        /// کلید پیشفرض 
        /// </summary>
        public static readonly string BaseKey = "Permissions:";

        /// <summary>
        /// مدت زمان ماندگاری در ردیس
        /// </summary>
        public static readonly int ExpMin = 15;


        /// <summary>
        /// تولید کلید
        /// </summary>
        /// <returns></returns>
        private static string GetKey(long userId) => $"{BaseKey}User{userId}";



        /// <summary>
        /// گرفتن دسترسی ها از ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="guid">توکن موقت عضو</param>
        public static async Task<List<string>> GetPermission(this IRedisDatabase db, long userId)
        {
            try
            {
                var res = await db.GetAsync<List<string>>(GetKey(userId));
                return res;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// افزودن دسترسی ها به ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <param name="model">دسترسی ها</param>
        /// <returns></returns>
        public static async Task<bool> SetPermission(this IRedisDatabase db, long userId, List<string> model, int? expMin = null)
        {
            try
            {
                return await db.AddAsync(GetKey(userId), model, DateTimeOffset.Now.AddMinutes(expMin ?? ExpMin));
            }
            catch
            {
                return false;
            }
        }




        /// <summary>
        /// حذف دسترسی ها از ردیس
        /// </summary>
        /// <param name="db">دیتابیس ردیس</param>
        /// <returns></returns>
        public static async Task<bool> RemovePermission(this IRedisDatabase db, long userId)
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
