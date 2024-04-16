using Application.DTOs.Users;
using Microsoft.AspNetCore.Http;

namespace Application.SessionServices
{
    /// <summary>
    /// مدیریت اطلاعات کاربر درون سشن
    /// </summary>
    public static class SessionUserManager
    {
        public static readonly string Key = "User";


        public static UserSessionDTO GetUser(this ISession session)
        {
            return session.Get<UserSessionDTO>(Key);
        }


        public static void SetUser(this ISession session, UserSessionDTO value)
        {
            session.Set<UserSessionDTO>(Key, value);
        }



        public static void RemoveUser(this ISession session)
        {
            session.Remove(Key);
        }

    }
}
