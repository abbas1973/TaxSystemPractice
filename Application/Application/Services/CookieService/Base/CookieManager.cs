using Microsoft.AspNetCore.Http;

namespace Application.CookieServices
{
    /// <summary>
    /// گرفتن کوکی فعلی
    /// </summary>
    public class CookieManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public HttpContext HttpContext;
        public HttpResponse Response;
        public HttpRequest Request;
        public IRequestCookieCollection RequestCookies;
        public IResponseCookies ResponseCookies;

        public CookieManager(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
            HttpContext = httpContextAccessor.HttpContext;
            Request = HttpContext.Request;
            Response = HttpContext.Response;
            RequestCookies = Request.Cookies;
            ResponseCookies = Response.Cookies;
        }
    }
}
