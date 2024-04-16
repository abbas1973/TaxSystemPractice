using Application.Filters;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Description("داشبورد")]
    [UserAuthorize(AuthorizeType.NeedAuthentication)]
    public class DashboardController : MyBaseController
    {
        #region Constructors
        public DashboardController() : base()
        {
        }
        #endregion


        #region داشبورد
        [Description("داشبورد")]
        public IActionResult Index()
        {
            return View();
        }
        #endregion





    }
}