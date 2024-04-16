using Application.Features.Web.Invoices;
using Application.Filters;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Application.Utilities;
using Domain.Enums;
using Application.Features.Base;
using Domain.Entities;
using DocumentFormat.OpenXml.Office2010.Excel;
using Application.Exceptions;

namespace Web.Areas.AuthSystem.Controllers
{
    [Area("TaxSystem")]
    [Description("مدیریت صورتحساب ها")]
    [UserAuthorize(AuthorizeType.NeedPermission)]
    public class InvoicesController : MyBaseController
    {
        #region Constructors
        public InvoicesController() : base()
        {
        }
        #endregion



        #region نمایش همه
        [HttpGet]
        [Description("لیست صورتحساب ها")]
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

            return View();
        }


        /// <summary>
        /// لیست داده مورد نیاز برای دیتاتیبل
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("لیست صورتحساب ها")]
        public async Task<ActionResult> Index(InvoiceGetListQuery query)
        {
            var res = await Mediator.Send(query);
            return Json(res);
        }
        #endregion


        #region جزییات
        [HttpGet]
        [Description("جزییات صورتحساب")]
        public async Task<IActionResult> GetById(long id)
        {
            var res = await Mediator.Send(new GetByIdQuery<Invoice, InvoiceGetByIdDTO>(id));
            if(!res.IsSuccess)
                return Json(res);
            return PartialView("_Details", res.Value);
        }
        #endregion


        #region ویرایش
        /// <summary>
        /// لود کردن فرم ایجاد در مدال
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("افزودن صورتحساب")]
        public async Task<IActionResult> Edit(long id)
        {
            var res = await Mediator.Send(new InvoiceGetEditDTOQuery(id));
            if (res is null)
                throw new NotFoundException("صورتحساب مورد نظر یافت نشد!");

            var payTypes = EnumExtensions.ToEnumViewModel<InvoicePayType>();
            ViewBag.InvoicePayTypes = new SelectList(payTypes, "Id", "Title", (int)res.PayType);

            var invoiceTypes = EnumExtensions.ToEnumViewModel<TaxInvoiceType>();
            ViewBag.InvoiceTypes = new SelectList(invoiceTypes, "Id", "Title", (int)res.TaxInvoiceType);

            var invoicePatterns = EnumExtensions.ToEnumViewModel<TaxInvoicePattern>();
            ViewBag.InvoicePatterns = new SelectList(invoicePatterns, "Id", "Title", (int)res.TaxInvoicePattern);

            var invoiceSubjectTypes = EnumExtensions.ToEnumViewModel<TaxInvoiceSubjectType>();
            ViewBag.InvoiceSubjects = new SelectList(invoiceSubjectTypes, "Id", "Title", (int)res.TaxInvoiceSubject);

            var BuyerTypes = EnumExtensions.ToEnumViewModel<BuyerType>();
            ViewBag.BuyerTypes = new SelectList(BuyerTypes, "Id", "Title", (int)res.BuyerIsRealOrLegal);

            return PartialView("_Edit", res);
        }


        [HttpPost]
        [Description("ویرایش صورتحساب")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InvoiceUpdateCommand command)
        {
            var res = await Mediator.Send(command);
            return Json(res);
        }
        #endregion


        #region حذف
        [HttpDelete]
        [Description("حذف صورتحساب")]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await Mediator.Send(new InvoiceDeleteCommand(id));
            return Json(res);
        }
        #endregion


        #region گزارش اکسل
        [HttpGet]
        [Description("گزارش اکسل")]
        public async Task<IActionResult> Export(InvoiceExcelReportsQuery query)
        {
            var wb = await Mediator.Send(query);
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"لیست صورت حساب ها.xlsx");
            }
        }
        #endregion
    }
}
