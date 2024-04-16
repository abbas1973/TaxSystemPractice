using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Redis.Services;
using Application.SessionServices;
using Application.Exceptions;
using System.Reflection;
using Application.DTOs.Claims;
using System.ComponentModel;
using Application.CookieServices;
using Microsoft.Extensions.Logging;
using Application.Repositories;

namespace Application.Filters
{
    /// <summary>
    /// بررسی وجود کاربر درون سشن به منظور لاگین
    /// </summary>

    public class UserAuthorize : ActionFilterAttribute
    {
        #region Constructors and Fields
        /// <summary>
        /// نوع احراز هویت درخواستی
        /// </summary>
        public AuthorizeType AuthorizeType { get; set; }

        /// <summary>
        /// اگر نوع احراز هویت نیاز به دسترسی اختصاصی بود،
        /// چه دسترسی هایی لازم است تا کاربر احراز هویت شود.
        /// <para>
        /// حداقل یکی از دسترسی ها کافیست
        /// </para>
        /// </summary>
        protected List<string> AllowedClaims { get; set; }


        /// <summary>
        /// آیا چک شود که کاربر کلمه عبور را تغییر داده است یا خیر؟
        /// </summary>
        protected bool CheckPasswordChange { get; set; }


        /// <summary>
        /// بررسی دسترسی کاربر
        /// </summary>
        public UserAuthorize(AuthorizeType authorizeType = AuthorizeType.NeedPermission, bool checkPasswordChange = true, params string[] allowedClaims)
        {
            AuthorizeType = authorizeType;
            AllowedClaims = allowedClaims.ToList() ?? new List<string>();
            CheckPasswordChange = checkPasswordChange;
        } 
        #endregion



        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            #region آدرس درخواست شده
            var url = context.HttpContext.Request.Path.ToString()
                            + context.HttpContext.Request.QueryString;
            #endregion

            #region استخراج مقادیر مورد نیاز
            var controllerObj = context.Controller as Controller;
            var Redis = controllerObj?.HttpContext.RequestServices.GetService<IRedisManager>();
            var _logger = controllerObj?.HttpContext.RequestServices.GetService<ILogger<UserAuthorize>>();

            var path = context.HttpContext.Request.Path;
            var endpoint = context.HttpContext.GetEndpoint();
            var User = controllerObj?.HttpContext.Session.GetUser();
            #endregion

            #region بررسی لاگین کاربر و انتقال به صفحه لاگین
            if (User == null)
                throw new UnAuthorizedException("جهت دسترسی به این صفحه، ابتدا وارد حساب کاربری خود شوید!"
                    ,retUrl: url);
            #endregion

            #region لاگین شخص دیگری با این یوزرنیم - اگر توکن کاربر با توکن درون ردیس یکی نباشد کاربر به صفحه لاگین منتقل می شود
            //var cookieToken = controllerObj?.HttpContext.GetCookieUserToken();
            //var redisToken = Redis.db.GetLoginToken(User.Id).Result;
            //if (string.IsNullOrEmpty(cookieToken) || cookieToken != redisToken)
            //{
            //    _logger.LogInformation($"کاربر \"{User.Name}\" با شناسه \"{User.Id}\" به دلیل ورود شخصی دیگر به حساب کاربری، از حساب کاربری خود خارج شد.");
            //    controllerObj?.HttpContext.Session.RemoveUser();
            //    throw new UnAuthorizedException("به دلیل ورود شخصی دیگر به حساب کاربری  شما، از حساب کاربری خود خارج شدید!"
            //        , retUrl: url);
            //}
            #endregion


            #region آیا کاربر تلفن همراه را تایید کرده است؟
            if (CheckPasswordChange && !User.PasswordIsChanged)
            {
                context.Result = new RedirectToActionResult("ConfirmMobile", "Authentication", new { area = "" });
                return;
            }
            #endregion

            #region آیا کاربر تغییر رمز را انجام داده؟
            if (CheckPasswordChange && !User.PasswordIsChanged)
            {
                context.Result = new RedirectToActionResult("ChangePassword", "Profile", new { area = "Admin" });
                return;
            }
            #endregion


            #region بررسی دسترسی کاربر به منو مشخص شده
            #region استخراج اطلاعات اریا، کنترلر و اکشن
            if (endpoint == null || !path.HasValue)
                throw new NotFoundException("مسیر درخواست شده یافت نشد!");

            var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (descriptor == null)
                throw new NotFoundException("مسیر درخواست شده یافت نشد!");

            string areaName = descriptor.ControllerTypeInfo
                            .GetCustomAttribute<AreaAttribute>().RouteValue;
            string controllerName = descriptor.ControllerName.Replace(nameof(Controller), "");
            string actionName = descriptor.MethodInfo.Name;
            string actionNameFa = descriptor.MethodInfo.GetCustomAttributes<DescriptionAttribute>(inherit: true).FirstOrDefault()?.Description ?? actionName;
            controllerObj.ViewBag.PageTitle = actionNameFa ?? actionName;
            #endregion

            #region اگر نوع احراز هویت فقط لاگین بود مجاز است
            if (AuthorizeType == AuthorizeType.NeedAuthentication)
                return;
            #endregion

            #region ایجاد کلایم و دسترسی های پلکانی تا بالاترین سطح
            var claimDTO = new ClaimDTO()
            {
                Name = actionNameFa,
                IsController = false,
                Controller = controllerName,
                Action = actionName,
                Area = areaName
            };
            var sections = claimDTO.Claim.Split('.');
            var AllowedScopes = new List<string> { "Full", claimDTO.Claim };
            for (int i = 0; i < sections.Length - 1; i++)
            {
                var tmp = sections.Take(i + 1);
                var prefix = string.Join('.', tmp);
                AllowedScopes.Add($"{prefix}.Full");
            }
            AllowedClaims.AddRange(AllowedScopes);
            #endregion

            #region بررسی دسترسی به کلایم مورد نظر
            #region بررسی دسترسی از ردیس
            var claims = Redis.db.GetUserClaims(User.Id).Result;
            bool hasPermission = claims.Any(x => AllowedClaims.Contains(x));
            #endregion

            #region بررسی دسترسی از دیتابیس
            //var _uow = controllerObj?.HttpContext.RequestServices.GetService<IUnitOfWork>();
            //var _roleIds = _uow.UserRoles.GetUserRoleIds(User.Id).Result;
            //bool hasPermission = _uow.RoleClaims.Any(x => _roleIds.Contains(x.RoleId) && AllowedClaims.Contains(x.Claim)); 
            #endregion

            if (!hasPermission)
            {
                _logger.LogWarning($"کاربر \"{User.Name}\" با شناسه \"{User.Id}\" به صفحه درخواست شده دسترسی ندارد!");
                throw new ForbiddenException("شما به صفحه درخواست شده دسترسی ندارید!");
            }
            #endregion
            #endregion

            return;
        }


    }


}
