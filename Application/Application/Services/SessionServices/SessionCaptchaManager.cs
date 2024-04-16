using Microsoft.AspNetCore.Http;

namespace Application.SessionServices
{
    /// <summary>
    /// مدیریت کپچا درون سشن
    /// </summary>
    public static class SessionCaptchaManager
    {
        public static readonly string Key = "Captcha";


        public static string GetCaptcha(this ISession session)
        {
            return session.Get<string>(Key);
        }


        public static void SetCaptcha(this ISession session, string value)
        {
            session.Set<string>(Key, value);
        }



        public static void RemoveCaptcha(this ISession session)
        {
            session.Remove(Key);
        }

    }
}
