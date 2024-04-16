using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Provinces
{
    #region Request
    public class ProvinceDeleteCommand
    : IRequest<BaseResult>, IBaseEntityDTO
    {
        #region Constructors
        public ProvinceDeleteCommand(long id)
        {
            Id = id;
        }
        #endregion


        #region Properties
        [Display(Name = "شناسه")]
        public long Id { get; set; }
        #endregion
    }
    #endregion



    #region Handler
    public class ProvinceDeleteQueryHandler : IRequestHandler<ProvinceDeleteCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        public ProvinceDeleteQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }


        public async Task<BaseResult> Handle(ProvinceDeleteCommand request, CancellationToken cancellationToken)
        {
            var deletedProvinces = await _uow.Provinces.ExecuteDeleteAsync(x => x.Id == request.Id);
            if (deletedProvinces == 0)
                throw new NotFoundException("استان مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
