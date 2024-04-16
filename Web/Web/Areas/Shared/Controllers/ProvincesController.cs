using Application.Features.Base;
using Application.Features.Web.Provinces;
using Application.Filters;
using Domain.Entities;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Web.Areas.Shared.Controllers
{
    [Area("Shared")]
    [Description("مدیریت استان ها")]
    [UserAuthorize(AuthorizeType.NeedPermission)]
    public class ProvincesController : MyBaseController
    {


        #region نمایش همه
        [HttpGet]
        [Description("نمایش لیست استان ها")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        /// <summary>
        /// لیست داده مورد نیاز برای دیتاتیبل
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("لیست استان ها")]
        public async Task<ActionResult> Index(ProvinceGetListQuery query)
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
        [Description("افزودن استان")]
        public async Task<IActionResult> Create()
        {
            return PartialView("_Create");
        }


        [HttpPost]
        [Description("افزودن استان")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProvinceCreateCommand command)
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
        [Description("ویرایش استان")]
        public async Task<IActionResult> Edit(long id)
        {
            var province = await Mediator.Send(new ProvinceGetUpdateDTOQuery(id));
            return PartialView("_Edit", province);
        }


        [HttpPut]
        [Description("ویرایش استان")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProvinceUpdateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion



        #region تغییر وضعیت فعال بودن یا نبودن استان 
        [HttpPost]
        [Description("تغییر وضعیت فعال/غیرفعال")]
        public async Task<IActionResult> ToggleEnable(long id)
        {
            var res = await Mediator.Send(new ToggleEnableCommand<Province>(id));
            return Json(res);
        }
        #endregion



        #region حذف
        [HttpDelete]
        [Description("حذف استان")]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await Mediator.Send(new ProvinceDeleteCommand(id));
            return Json(res);
        }
        #endregion



        #region گرفتن لیست استان ها
        [Description("گرفتن لیست استان ها")]
        [UserAuthorize(AuthorizeType.NeedAuthentication)]
        public async Task<IActionResult> GetSelectList(ProvinceGetSelectListQuery query)
        {
            var res = await Mediator.Send(query);
            return Json(res);
        }
        #endregion
    }
}
