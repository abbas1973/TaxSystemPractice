using Application.Features.Web.Provinces;
using Application.Features.Web.Roles;
using Application.Features.Web.Users;
using Application.Filters;
using Domain.Entities;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Web.Areas.AuthSystem.Controllers
{
    [Area("AuthSystem")]
    [Description("مدیریت کاربران")]
    [UserAuthorize(AuthorizeType.NeedPermission)]
    public class UsersController : MyBaseController
    {
        #region Constructors
        public UsersController() : base()
        {
        }
        #endregion



        #region نمایش همه
        [HttpGet]
        [Description("لیست کاربران")]
        public async Task<IActionResult> Index()
        {
            var roles = await Mediator.Send(new RoleGetSelectListQuery());
            ViewData["RoleId"] = new SelectList(roles, "Id", "Title");
            return View();
        }


        /// <summary>
        /// لیست داده مورد نیاز برای دیتاتیبل
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("لیست کاربران")]
        public async Task<ActionResult> Index(UserGetListQuery query)
        {
            var res = await Mediator.Send(query);
            return Json(res);
        }
        #endregion



        #region ایجاد
        /// <summary>
        /// لود کردن فرم ایجاد در مدال
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("افزودن کاربر")]
        public async Task<IActionResult> Create()
        {
            #region استان ها
            var provinces = await Mediator.Send(new ProvinceGetSelectListQuery(true));
            ViewData["Provinces"] = new SelectList(provinces, "Id", "Title");
            #endregion

            #region نقش ها
            var roles = await Mediator.Send(new RoleGetSelectListQuery());
            ViewData["Roles"] = roles;
            #endregion

            return PartialView("_Create");
        }


        [HttpPost]
        [Description("افزودن کاربر")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion



        #region ویرایش
        /// <summary>
        /// لود کردن فرم ویرایش در مدال
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("ویرایش کاربر")]
        public async Task<IActionResult> Edit(long id)
        {
            var user = await Mediator.Send(new UserGetUpdateDTOQuery(id));

            #region استان ها
            var provinces = await Mediator.Send(new ProvinceGetSelectListQuery(true));
            ViewData["Provinces"] = new SelectList(provinces, "Id", "Title", user.ProvinceId);
            #endregion

            #region نقش ها
            var roles = await Mediator.Send(new RoleGetSelectListQuery());
            ViewData["Roles"] = roles;
            #endregion

            return PartialView("_Edit", user);
        }


        [HttpPut]
        [Description("ویرایش کاربر")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion



        #region تغییر کلمه عبور
        /// <summary>
        /// لود کردن فرم تغییر کلمه عبور در مدال
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("ویرایش کلمه عبور کاربر")]
        public IActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }


        [HttpPost]
        [Description("ویرایش کلمه عبور کاربر")]
        public async Task<IActionResult> ChangePassword(UserChangePasswordCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }

        #endregion



        #region تغییر وضعیت فعال بودن یا نبودن کاربر 
        [HttpPost]
        [Description("تغییر وضعیت فعال/غیرفعال")]
        public async Task<IActionResult> ToggleEnable(long id)
        {
            var res = await Mediator.Send(new UserToggleEnableCommand(id));
            return Json(res);
        }
        #endregion



        #region حذف
        [HttpDelete]
        [Description("حذف کاربر")]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await Mediator.Send(new UserDeleteCommand(id));
            return Json(res);
        }
        #endregion

    }
}
