using Application.DTOs;

namespace Application.Contracts
{
    public interface IJwtManager
    {

        /// <summary>
        /// گرفتن توکن از هدر ریکوئست
        /// </summary>
        /// <returns></returns>
        string GetHeaderToken();


        // <summary>
        /// گرفتن آیدی کاربر از توکن
        /// </summary>
        /// <returns></returns>
        long? GetUserId(string jwt = null);


        /// <summary>
        /// گرفتن نام کاربر از توکن
        /// </summary>
        /// <returns></returns>
        public string GetUserName(string jwt = null);


        #region توابع توکن دسترسی
        /// <summary>
        /// ایجاد یک توکن دسترسی
        /// </summary>
        /// <param name="userId"> آیدی کاربر و اپلیکیشن</param>
        /// <returns></returns>
        TokenResponseDTO GenerateAccessToken(long? userId, string name, string tokenKey, double? expMin = null);



        /// <summary>
        /// ولیدیت کردن توکن
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        bool ValidateAccessToken(string token); 
        #endregion




        #region توابع رفرش توکن

        /// <summary>
        /// ایجاد توکن دسترسی با استفاده از اطلاعات کاربر
        /// </summary>
        /// <returns></returns>
        TokenResponseDTO GenerateRefreshToken(long? userId, string tokenKey, double? expMin = null);


        /// <summary>
        /// ولیدیت کردن توکن دسترسی
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        bool ValidateRefreshToken(string token);
        #endregion




        /// <summary>
        ///  گرفتن اطلاعات مخفی درون توکن
        /// </summary>
        /// <param name="token">توکن</param>
        /// <param name="claimType">پارامتر مورد نیاز</param>
        /// <returns></returns>
        string GetClaim(string token, string claimType);




    }
}
