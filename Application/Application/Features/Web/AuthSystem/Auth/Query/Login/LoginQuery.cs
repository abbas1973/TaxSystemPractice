using Application.CookieServices;
using Application.DTOs;
using Application.DTOs.Users;
using Application.Repositories;
using Application.SessionServices;
using Domain.Entities;
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Redis.Services;
using System.Linq.Expressions;
using Utilities;

namespace Application.Features.Web.Auth
{
    #region Request
    public class LoginQuery
    : IRequest<BaseResult<bool>>
    {
        #region Constructors
        public LoginQuery()
        {
        }
        #endregion


        #region Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
        public string RetUrl { get; set; }
        #endregion


        #region توابع
        public Expression<Func<User, bool>> Getfilter()
        {
            var hashedPassword = Password.GetHash();
            var filter = PredicateBuilder.New<User>(true);
            filter.And(x => x.Username == Username);
            filter.And(x => x.Password == hashedPassword);
            return filter;
        }
        #endregion
    }
    #endregion



    #region Handler
    public class LoginQueryHandler : IRequestHandler<LoginQuery, BaseResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRedisManager _redis;
        private readonly IHttpContextAccessor _accessor;
        private readonly ISession _session;
        private readonly ILogger<LoginQueryHandler> _logger;
        public LoginQueryHandler(IUnitOfWork uow, IHttpContextAccessor accessor, ILogger<LoginQueryHandler> logger, IRedisManager redis = null)
        {
            _uow = uow;
            _accessor = accessor;
            _session = accessor.HttpContext.Session;
            _logger = logger;
            _redis = redis;
        }


        public async Task<BaseResult<bool>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            #region بررسی کپچا در صورت وجود
            var captcha = _session.GetCaptcha();
            if (string.IsNullOrEmpty(captcha) || captcha != request.Captcha)
            {
                _session.RemoveCaptcha();
                _logger.LogWarning($"کد امنیتی صحیح نیست! نام کاربری: {request.Username} , کد امنیتی: {request.Captcha}");
                return new BaseResult<bool>(false, "کد امنیتی صحیح نیست!");
            }
            _session.RemoveCaptcha();
            #endregion

            #region پیدا کردن کاربر
            var user = await _uow.Users.GetOneDTOAsync(
                    UserSessionDTO.Selector,
                    request.Getfilter());
            if (user == null)
                return new BaseResult<bool>(false, "نام کاربری یا کلمه عبور اشتباه است!");

            if (!user.IsEnabled)
                return new BaseResult<bool>(false, "حساب کاربری شما غیر فعال شده است. لطفا با مدیر سایت تماس بگیرید!");

            #endregion

            #region اطلاعات سشن، ردیس و کوکی
            _session.SetUser(user);

            // افزودن توکن کاربر به ردیس برای جلوگیری از لاگین همزمان 2 نفر با یک اکانت
            var token = await _redis.db.SetLoginToken(user.Id);

            // افزودن توکن به کوکی
            _accessor.HttpContext.SetCookieUserToken(token);
            #endregion

            #region قراردادن اطلاعات دسترسی کاربر درون ردیس
            // حذف اطلاعات از ردیس
            _ = await _redis.db.RemoveUserClaims(user.Id);

            // گرفتن نقش های کاربر
            var roleIds = await _uow.UserRoles.GetUserRoleIds(user.Id);

            #region گرفتن کلایم های نقش ها
            if (roleIds != null && roleIds.Any())
            {
                var claims = await _uow.RoleClaims.GetDTOAsync(
                        x => x.Claim,
                        x => roleIds.Contains(x.RoleId));

                //ذخیره اطلاعات در ردیس
                _ = await _redis.db.SetUserClaims(user.Id, claims);
            }
            #endregion
            #endregion

            return new BaseResult<bool>(value: user.MobileConfirmed);
        }
    }

    #endregion
}
