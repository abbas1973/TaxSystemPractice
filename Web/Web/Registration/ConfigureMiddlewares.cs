using Application.Middleware;

namespace Registration
{
    public static class ConfigureMiddlewares
    {
        /// <summary>
        /// استفاده از میدلور های کاستوم توسط app
        /// IApplicationBuilder
        /// </summary>
        public static IApplicationBuilder UseCustomMiddlewares(this WebApplication app)
        {

            #region Exception Handler
            app.UseExceptionHandler(
                      new ExceptionHandlerOptions()
                      {
                          AllowStatusCode404Response = true,
                          ExceptionHandlingPath = "/error"
                      }
                  );
            #endregion

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseErrorHandlerMiddleware();

            #region امنیت
            #region تنظیم هدر برای جلوگیری از حملات
            app.Use(async (context, next) =>
            {
                // برای جلوگیری از iframe شدن صفحات سایت و براي مقابله در برابر حملات ClickJacking
                context.Response.Headers.Add("X-Frame-Options", "DENY");

                // جلوگیری از حملات xss
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");

                // جلوگیری از MIME-Sniffing و تغییر پسوند فایل ها
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                // جلوگیری از باز کردن فایل های خارج از حالت لوکال
                // اگر لینکی به سایتی مثل جی کوئری داشتیم باید اینجا اضافه کنیم.
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; style-src 'self' 'unsafe-inline'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; font-src 'self' https://fonts.gstatic.com; connect-src 'self' wss:");

                context.Response.Headers.Remove("Server");
                await next();
            });
            #endregion
            #endregion


            app.UseStaticFiles();
            app.UseRouting();

            app.UseCookiePolicy();
            app.UseSession();

            #region برای کش کردن ریسپانس ها
            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //        {
            //            Public = true,
            //            MaxAge = TimeSpan.FromSeconds(1200)
            //        };
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "Accept-Encoding" };
            //    await next();
            //});
            //app.UseResponseCaching();
            #endregion


            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Index}/{id?}");

            return app;
        }
    }
}
