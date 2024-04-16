using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public partial class MyBaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    }
}
