using Application.Features.Web.Companies;
using Application.Features.Web.Invoices;
using Application.Filters;
using Application.Utilities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Web.Controllers;

namespace Web.Areas.TaxSystem.Controllers
{
    [Area("TaxSystem")]
    [Description("افزودن صورتحساب")]
    [UserAuthorize(AuthorizeType.NeedPermission)]
    public class AddInvoiceController : MyBaseController
    {
        #region ایجاد
        /// <summary>
        /// لود کردن فرم ایجاد در مدال
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("افزودن صورتحساب")]
        public async Task<IActionResult> Index()
        {
            var payTypes = EnumExtensions.ToEnumViewModel<InvoicePayType>();
            ViewBag.InvoicePayTypes = new SelectList(payTypes, "Id", "Title");

            var invoiceTypes = EnumExtensions.ToEnumViewModel<TaxInvoiceType>();
            ViewBag.InvoiceTypes = new SelectList(invoiceTypes, "Id", "Title");

            var invoicePatterns = EnumExtensions.ToEnumViewModel<TaxInvoicePattern>();
            ViewBag.InvoicePatterns = new SelectList(invoicePatterns, "Id", "Title");

            var invoiceSubjectTypes = EnumExtensions.ToEnumViewModel<TaxInvoiceSubjectType>();
            ViewBag.InvoiceSubjects = new SelectList(invoiceSubjectTypes, "Id", "Title");

            var sendingStatuses = EnumExtensions.ToEnumViewModel<InvoiceSendingStatus>();
            ViewBag.InvoiceSendingStatuses = new SelectList(sendingStatuses, "Id", "Title");

            var BuyerTypes = EnumExtensions.ToEnumViewModel<BuyerType>();
            ViewBag.BuyerTypes = new SelectList(BuyerTypes, "Id", "Title");

            return View("Index");
        }


        [HttpPost]
        [Description("افزودن صورتحساب")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(InvoiceCreateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion
    }
}
