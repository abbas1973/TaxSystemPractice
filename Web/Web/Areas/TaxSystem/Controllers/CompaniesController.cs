using Application.Features.Web.Companies;
using Application.Filters;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Web.Areas.TaxSystem.Controllers
{
    [Area("TaxSystem")]
    [Description("مدیریت شرکت ها")]
    [UserAuthorize(AuthorizeType.NeedPermission)]
    public class CompaniesController : MyBaseController
    {


        #region نمایش همه
        [HttpGet]
        [Description("نمایش لیست شرکت ها")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        /// <summary>
        /// لیست داده مورد نیاز برای دیتاتیبل
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("لیست شرکت ها")]
        public async Task<ActionResult> Index(CompanyGetListQuery query)
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
        [Description("افزودن شرکت")]
        public async Task<IActionResult> Create()
        {
            return PartialView("_Create");
        }


        [HttpPost]
        [Description("افزودن شرکت")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCreateCommand command)
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
        [Description("ویرایش شرکت")]
        public async Task<IActionResult> Edit(long id)
        {
            var Company = await Mediator.Send(new CompanyGetUpdateDTOQuery(id));
            return PartialView("_Edit", Company);
        }


        [HttpPut]
        [Description("ویرایش شرکت")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyUpdateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion        



        #region حذف
        [HttpDelete]
        [Description("حذف شرکت")]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await Mediator.Send(new CompanyDeleteCommand(id));
            return Json(res);
        }
        #endregion
    }
}
