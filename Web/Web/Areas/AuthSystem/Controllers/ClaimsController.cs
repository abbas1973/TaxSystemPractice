using Application.Features.Web.Claims;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection;

namespace Web.Areas.AuthSystem.Controllers
{
    [Area("AuthSystem")]
    [Description("مدیریت کلایم ها")]
    public class ClaimsController : MyBaseController
    {
        [Description("گرفتن لیست کلایم ها")]
        public async Task<IActionResult> Index()
        {
            var res = await Mediator.Send(new ClaimGetAllQuery(Assembly.GetExecutingAssembly()));
            return Json(res);
        }


    }
}
