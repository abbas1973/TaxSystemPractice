using Application.Features.Web.Claims;
using Application.Features.Web.Provinces;
using Application.Features.Web.Roles;
using Application.Filters;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Reflection;

namespace Web.Areas.AuthSystem.Controllers
{
    [Area("AuthSystem")]
    [Description("مدیریت نقش ها")]
    [UserAuthorize(AuthorizeType.NeedPermission)]
    public class RolesController : MyBaseController
    {
        #region Constructors
        public RolesController() : base()
        {
        }
        #endregion



        #region نمایش همه
        [HttpGet]
        [Description("لیست نقش ها")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        /// <summary>
        /// لیست داده مورد نیاز برای دیتاتیبل
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("لیست نقش ها")]
        public async Task<ActionResult> Index(RoleGetListQuery query)
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
        [Description("افزودن نقش")]
        public async Task<IActionResult> Create()
        {
            #region کلایم ها
            var claim = await Mediator.Send(new ClaimGetAllQuery(Assembly.GetExecutingAssembly()));
            ViewData["Claim"] = claim;
            #endregion

            return PartialView("_Create");
        }


        [HttpPost]
        [Description("افزودن نقش")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleCreateCommand command)
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
        [Description("ویرایش نقش")]
        public async Task<IActionResult> Edit(long id)
        {
            var role = await Mediator.Send(new RoleGetUpdateDTOQuery(id));
            ViewData["selectedClaims"] = role.Claims;

            #region کلایم ها
            var claim = await Mediator.Send(new ClaimGetAllQuery(Assembly.GetExecutingAssembly()));
            ViewData["Claim"] = claim;
            #endregion

            return PartialView("_Edit", role);
        }


        [HttpPut]
        [Description("ویرایش نقش")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleUpdateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion



        #region تغییر وضعیت فعال بودن یا نبودن نقش 
        [HttpPost]
        [Description("تغییر وضعیت فعال/غیرفعال")]
        public async Task<IActionResult> ToggleEnable(long id)
        {
            var res = await Mediator.Send(new RoleToggleEnableCommand(id));
            return Json(res);
        }
        #endregion



        #region حذف
        [HttpDelete]
        [Description("حذف نقش")]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await Mediator.Send(new RoleDeleteCommand(id));
            return Json(res);
        }
        #endregion

    }
}
