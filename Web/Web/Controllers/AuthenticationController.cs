using Application.Contracts;
using Application.CookieServices;
using Application.Features.Web.Auth;
using Application.SessionServices;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// صفحه لاگین کاربران با یوزر پس
    /// </summary>
    public class AuthenticationController : MyBaseController
    {
        private readonly ISession session;
        public AuthenticationController(IHttpContextAccessor _httpContextAccessor)
        {
            session = _httpContextAccessor.HttpContext.Session;
        }


        #region لاگین به پنل کاربر
        /// <summary>
        /// لاگین
        /// </summary>
        public IActionResult Index()
        {
            var User = session.GetUser();
            if (User != null)
                return RedirectToAction("index", "Dashboard", new { area = "Admin" });
            HttpContext.RemoveSettingCookie();
            return View();
        }




        /// <summary>
        /// لاگین با یوزر پس
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(LoginQuery query)
        {
            var res = await Mediator.Send(query);
            if (!res.IsSuccess)
            {
                ViewBag.Error = string.Join(" | ", res.Errors);
                return View();
            }
            else
            {
                if (!res.Value) // تایید موبایل
                    return RedirectToAction("ConfirmMobile");
            }

            if (!string.IsNullOrEmpty(query.RetUrl))
            {
                var isLocal = Url.IsLocalUrl(query.RetUrl);
                if (isLocal)
                    return Redirect(query.RetUrl);
            }
            return RedirectToAction("index", "Dashboard", new { area = "Admin" });
        }
        #endregion





        #region ثبت نام کاربران
        #region ثبت نام
        ///// <summary>
        ///// ثبت نام
        ///// </summary>
        //public IActionResult Register()
        //{
        //    return View();
        //}




        ///// <summary>
        ///// ثبت نام
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterDTO model)
        //{
        //    var res = await authManager.Register(model);
        //    return Json(res);
        //}
        #endregion



        #region تایید موبایل
        //public IActionResult ConfirmMobile()
        //{
        //    var user = session.GetUser();
        //    if (user == null)
        //    {
        //        var model = new ErrorPageDTO
        //        {
        //            ErrorCode = "404",
        //            Title = "خطا",
        //            Description = "جهت تایید تلفن همراه، ابتدا وارد حساب کاربری خود شوید!"
        //        };
        //        return View("/Areas/Admin/Views/Shared/ErrorPage.cshtml", model);
        //    }
        //    return View();
        //}




        ///// <summary>
        ///// تایید کد احراز هویت
        ///// </summary>
        ///// <param name="Code">کد تایید</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ConfirmMobile(string Code)
        //{
        //    var user = session.GetUser();
        //    if (user == null)
        //    {
        //        var model = new ErrorPageDTO
        //        {
        //            ErrorCode = "404",
        //            Title = "خطا",
        //            Description = "جهت تایید تلفن همراه، ابتدا وارد حساب کاربری خود شوید!"
        //        };
        //        return View("/Areas/Admin/Views/Shared/ErrorPage.cshtml", model);
        //    }
        //    var res = authManager.ConfirmCode(user.Id, Code);
        //    if (!res.Status)
        //    {
        //        ViewBag.Error = res.Message;
        //        return View();
        //    }

        //    user.MobileConfirmed = true;
        //    session.RemoveUser();
        //    session.SetUser(user);
        //    return Redirect("/admin");
        //}
        #endregion




        #region ارسال مجدد کد تایید
        ///// <summary>
        ///// ارسال مجدد کد تایید
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResendCode()
        //{
        //    var user = session.GetUser();
        //    if (user == null)
        //        return Json(new { Status = false, Message = "جهت تایید تلفن همراه، ابتدا وارد حساب کاربری خود شوید!" });

        //    var res = await authManager.ResendCode(user.Id);
        //    return Json(res);
        //}
        #endregion
        #endregion





        #region خروج از حساب کاربری - logout
        public ActionResult Logout()
        {
            session.RemoveUser();
            HttpContext.RemoveSettingCookie();
            return RedirectToAction("index");
        }
        #endregion





    }
}