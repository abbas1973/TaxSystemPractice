using Application.DTOs;
using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Utilities;

namespace Api.Etka.Controllers
{

    /// <summary>
    /// مدیریت خطاها و تولید خروجی مناسب با استاتوس کد مناسب
    /// </summary>
    public class ErrorsController : Controller
    {

        [HttpGet]
        [Route("error")]
        public IActionResult GetError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            var errorModel = new ErrorPageDTO(500, "خطا!", exception);
            bool redirectToLogin = false;
            string retUrl = null;

            if (exception != null && exception is ErrorPageException httpException)
            {
                errorModel.ErrorCode = httpException.ErrorCode;
                errorModel.Title = httpException.Title;
                errorModel.Errors = httpException.Errors;
                redirectToLogin = httpException.RedirectToLogin;
                retUrl = httpException.RetUrl;
            }
            Response.StatusCode = errorModel.ErrorCode;

            // اگر درخواست از نوع اجکس بود
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new BaseResult(exception));
            }
            else // درخواست غیر اجکس
            {
                if (redirectToLogin)
                {
                    ViewData["Message"] = string.Join(" | ", errorModel.Errors);
                    ViewData["RetUrl"] = retUrl;
                    return View("~/Views/Authentication/Index.cshtml");
                }

                return View("ErrorPage", errorModel);
            }
                
        }



        [HttpPost]
        [Route("error")]
        public IActionResult PostError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            var errorModel = new ErrorPageDTO(500, "خطا!", exception);
            bool redirectToLogin = false;
            string retUrl = null;

            if (exception != null && exception is ErrorPageException httpException)
            {
                errorModel.ErrorCode = httpException.ErrorCode;
                errorModel.Title = httpException.Title;
                errorModel.Errors = httpException.Errors;
                redirectToLogin = httpException.RedirectToLogin;
                retUrl = httpException.RetUrl;
            }
            Response.StatusCode = errorModel.ErrorCode;

            // اگر درخواست از نوع اجکس بود
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new BaseResult(exception));
            }
            else // درخواست غیر اجکس
            {
                if (redirectToLogin)
                {
                    TempData["Message"] = string.Join(" | ", errorModel.Errors);
                    TempData["RetUrl"] = retUrl;
                    return View("~/Views/Authentication/Index.cshtml");
                }

                return View("ErrorPage", errorModel);
            }

        }



    }
}