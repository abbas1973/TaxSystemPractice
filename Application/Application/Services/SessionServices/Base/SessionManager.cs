using Microsoft.AspNetCore.Http;

namespace Application.SessionServices
{
    /// <summary>
    /// گرفتن سشن فعلی
    /// </summary>
    public class SessionManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public ISession Session;

        public SessionManager(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
            Session = httpContextAccessor.HttpContext.Session;
        }
    }
}
