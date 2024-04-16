using Application;
using Infrastructure;
using Application.CookieServices;
using Microsoft.AspNetCore.HttpOverrides;
using CookieExtensions = Application.CookieServices.CookieExtensions;

namespace Registration
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(
            this WebApplicationBuilder builder,
            IConfiguration configuration)
        {
            var services = builder.Services;

            #region رجیستر سرویس های پایه
            #region AddControllersWithViews
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation()
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
            services.AddRazorPages(); 
            #endregion

            //گرفتن httpcontext در کلاس لایبرری ها
            services.AddHttpContextAccessor();

            services.AddMySession(configuration);
            services.AddMyCookie();
            services.AddMyCache();

            // Cors
            services.AddCors(configuration);

            // کانفیگ هدر فرواردینگ برای ریدایرکت ها
            services.ConfigureForwardedHeader();


            #region هدر فوروارد
            services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                }); 
            #endregion

            services.AddAntiforgery();

            #region کانفیگ HSTS
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
            #endregion
            #endregion


            // context و uow
            services.RegisterInfrastructure(configuration);


            // کانفیگ های لایه اپلیکیشن
            builder.RegisterApplication(configuration);           
        }



        #region کانفیگ هدر فروارد برای ریدایرکت شدن ها
        public static void ConfigureForwardedHeader(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }
        #endregion


        #region Cors
        public static void AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsAllowed = configuration.GetSection("CorsAllowed").Get<string[]>().ToList();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                  .SetIsOriginAllowed(origin =>
                  {
                      if (string.IsNullOrWhiteSpace(origin)) return false;

                      if (corsAllowed == null || !corsAllowed.Any())
                          return true;

                      return corsAllowed.Any(url => origin.ToLower() == url || origin.ToLower().StartsWith(url + "/"));
                  });
            }));
        }
        #endregion


        #region افزودن سشن
        public static void AddMySession(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(configuration.GetValue<double?>("Setting:SessionTimeout") ?? 60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.Name = CookieExtensions.SessionCookieKey;
            });
        }
        #endregion


        #region افزودن کوکی
        public static void AddMyCookie(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            });
        }
        #endregion


        #region افزودن کش
        public static void AddMyCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = false;
            });
        }
        #endregion


        #region نام کوکی و هدر AntiForgeryKey برای جلوگیری از حملات csrf
        public static void AddAntiforgery(this IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = CookieExtensions.CsrfCookieKey;
                options.HeaderName = "_CSRF_header";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });
        }
        #endregion
    }
}
