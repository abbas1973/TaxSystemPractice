using Application.Features.Web.Profiles;
using Application.Features.Web.Provinces;
using Application.Filters;
using Application.SessionServices;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Web.Areas.Admin.Controllers
{
    /// <summary>
    /// پروفایل کاربر
    /// </summary>

    [Area("Admin")]
    [Description("پروفایل")]
    [UserAuthorize(AuthorizeType.NeedAuthentication)]
    public class ProfileController : MyBaseController
    {

        private readonly ISession session;
        public ProfileController(IHttpContextAccessor _httpContextAccessor)
        {
            session = _httpContextAccessor.HttpContext.Session;
        }



        #region ویرایش
        [HttpGet]
        [Description("ویرایش پروفایل")]
        public async Task<IActionResult> Edit()
        {
            var user = session.GetUser();
            var model = await Mediator.Send(new UserProfileGetUpdateDTOQuery(user.Id));

            #region استان ها
            var provinces = await Mediator.Send(new ProvinceGetSelectListQuery(true));
            ViewData["Provinces"] = new SelectList(provinces, "Id", "Title", model.ProvinceId);
            #endregion

            return View(model);
        }


        [HttpPut]
        [ValidateAntiForgeryToken]
        [Description("ویرایش پروفایل")]
        public async Task<IActionResult> Edit(UserProfileUpdateCommand model)
        {
            var res = await Mediator.Send(model);
            return Json(res);
        }

        #endregion


        #region تغییر کلمه عبور
        [HttpGet]
        [Description("تغییر کلمه عبور")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Description("تغییر کلمه عبور")]
        public async Task<IActionResult> ChangePassword(UserProfileChangePasswordCommand model)
        {
            var res = await Mediator.Send(model);
            return Json(res);
        }
        #endregion

    }
}
