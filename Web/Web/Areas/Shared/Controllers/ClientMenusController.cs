using Application.Features.Web.Users;
using Application.Filters;
using Application.SessionServices;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Areas.Global.Controllers
{
    [UserAuthorize(AuthorizeType.NeedAuthentication)]
    [Area("Shared")]
    public class ClientMenusController : MyBaseController
    {
        public ClientMenusController(IHttpContextAccessor _httpContextAccessor)
        {
        }


        public async Task<IActionResult> LoadMenu()
        {
            var user = HttpContext.Session.GetUser();
            var claims = await Mediator.Send(new UserGetClaimsQuery(user.Id));
            ViewBag.Claims = claims;
            return PartialView("_Menus");
        }
    }
}