//using Application.DTOs;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Web.Controllers
//{
//    public class BaseCrudController : MyBaseController
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }


//        public class BaseAppController<TGridDTO, TDetailsDTO, TCreateCommand, TUpdateCommand> : ControllerBase
//            where TGridDTO : IBaseEntityDTO
//            where TDetailsDTO : IBaseEntityDTO
//            where TCreateCommand : IRequest
//            where TUpdateCommand : IRequest
//        {

//            [HttpGet("{id}")]
//            public virtual async Task<IActionResult> GetAsync(int id)
//            {
//                Mediator.Send()
//                return Ok(await _service.GetByIdAsync(id));
//            }

//            [HttpGet]
//            public async Task<IActionResult> GetListAsync([DataSourceRequest] DataSourceRequest request, [FromQuery] TReadDto readRequest)
//            {
//                return Ok(await _service.GetDtoAsync(readRequest).ToDataSourceResultAsync(request));
//            }

//            [HttpPost]
//            public async Task<IActionResult> CreateAsync(TCreateDto request)
//            {
//                return Ok(await _service.CreateAsync(request));
//            }

//            [HttpPost("{id}")]
//            public async Task<IActionResult> UpdateAsync(int id, TUpdateDto request)
//            {
//                request.Id = id;
//                return Ok(await _service.UpdateAsync(request));
//            }

//            [HttpDelete("{id}")]
//            public async Task<IActionResult> DeleteAsync(int id)
//            {
//                var result = await _service.DeleteAsync(id);
//                return Ok(result);
//            }

//        }

//    }

//}
