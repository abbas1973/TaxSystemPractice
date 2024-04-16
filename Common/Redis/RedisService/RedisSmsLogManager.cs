//using Api.DTO.Category;
//using Domain.Entities;
//using Microsoft.AspNetCore.Http;
//using StackExchange.Redis;
//using StackExchange.Redis.Extensions.Core.Abstractions;

//namespace Api.Services.RedisService
//{
//    /// <summary>
//    /// پیامک های ارسالی
//    /// </summary>
//    public static class RedisSmsLogManager
//    {
//        /// <summary>
//        /// کلید پیشفرض 
//        /// </summary>
//        public static readonly string Key = "SmsLog";

//        /// <summary>
//        /// مدت زمان ماندگاری در ردیس
//        /// </summary>
//        public static readonly int ExpMin = 1440;




//        /// <summary>
//        /// گرفتن پیامک از ردیس
//        /// </summary>
//        /// <param name="db">دیتابیس ردیس</param>
//        /// <param name="count">تعداد ایتم های درخواستی</param>
//        public static async Task<List<SmsLog>> GetSmsLog(this IRedisDatabase db, int count = 1)
//        {
//            try
//            {
//                var res = await db.SortedSetRangeByScoreAsync<SmsLog>(Key, order: Order.Ascending, take: count);
//                if (res != null && res.Any())
//                    foreach (var item in res)
//                        await db.RemoveSmsLog(item);
//                return res.ToList();
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        /// <summary>
//        /// افزودن پیامک به ردیس
//        /// </summary>
//        /// <param name="db">دیتابیس ردیس</param>
//        /// <param name="sms">پیامک</param>
//        /// <returns></returns>
//        public static async Task<bool> SetSmsLog(this IRedisDatabase db, SmsLog sms)
//        {
//            try
//            {
//                var score = sms.SmsType == null ? 100 : (int)sms.SmsType;
//                return await db.SortedSetAddAsync(Key, sms, score);
//            }
//            catch
//            {
//                return false;
//            }
//        }




//        /// <summary>
//        /// حذف پیامک از ردیس
//        /// </summary>
//        /// <param name="db">دیتابیس ردیس</param>
//        /// <param name="sms">پیامک</param>
//        /// <returns></returns>
//        public static async Task<bool> RemoveSmsLog(this IRedisDatabase db, SmsLog sms)
//        {
//            try
//            {
//                return await db.SortedSetRemoveAsync(Key, sms);
//            }
//            catch
//            {
//                return false;
//            }
//        }



//    }
//}
