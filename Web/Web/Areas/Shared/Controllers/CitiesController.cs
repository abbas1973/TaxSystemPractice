using Application.Features.Base;
using Application.Features.Web.Cities;
using Application.Features.Web.Provinces;
using Application.Filters;
using Domain.Entities;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Web.Areas.Shared.Controllers
{
    [Area("Shared")]
    [Description("مدیریت شهر ها")]
    [UserAuthorize(AuthorizeType.NeedPermission)]
    public class CitiesController : MyBaseController
    {


        #region نمایش همه
        [HttpGet]
        [Description("نمایش لیست شهر ها")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        /// <summary>
        /// لیست داده مورد نیاز برای دیتاتیبل
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("لیست شهر ها")]
        public async Task<ActionResult> Index(CityGetListQuery query)
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
        [Description("افزودن شهر")]
        public async Task<IActionResult> Create()
        {
            #region استان ها
            var provinces = await Mediator.Send(new ProvinceGetSelectListQuery(true));
            ViewData["Provinces"] = new SelectList(provinces, "Id", "Title");
            #endregion

            return PartialView("_Create");
        }


        [HttpPost]
        [Description("افزودن شهر")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityCreateCommand command)
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
        [Description("ویرایش شهر")]
        public async Task<IActionResult> Edit(long id)
        {
            var city = await Mediator.Send(new CityGetUpdateDTOQuery(id));

            #region استان ها
            var provinces = await Mediator.Send(new ProvinceGetSelectListQuery(true));
            ViewData["Provinces"] = new SelectList(provinces, "Id", "Title", city.ProvinceId);
            #endregion

            return PartialView("_Edit", city);
        }


        [HttpPut]
        [Description("ویرایش شهر")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CityUpdateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion



        #region تغییر وضعیت فعال بودن یا نبودن شهر 
        [HttpPost]
        [Description("تغییر وضعیت فعال/غیرفعال")]
        public async Task<IActionResult> ToggleEnable(long id)
        {
            var res = await Mediator.Send(new ToggleEnableCommand<City>(id));
            return Json(res);
        }
        #endregion



        #region حذف
        [HttpDelete]
        [Description("حذف شهر")]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await Mediator.Send(new CityDeleteCommand(id));
            return Json(res);
        }
        #endregion



        #region جستجو شهر ها
        [HttpGet]
        [Description("جستجو شهر ها")]
        [UserAuthorize(AuthorizeType.NeedAuthentication)]
        public async Task<IActionResult> Search(CitySearchQuery query)
        {
            var res = await Mediator.Send(query);
            return Json(res);
        }
        #endregion



        #region گرفتن لیست شهر ها
        [HttpGet]
        [Description("گرفتن لیست شهر ها")]
        [UserAuthorize(AuthorizeType.NeedAuthentication)]
        public async Task<IActionResult> GetSelectList(CityGetSelectListQuery query)
        {
            var res = await Mediator.Send(query);
            return Json(res);
        }
        #endregion
    }
}
