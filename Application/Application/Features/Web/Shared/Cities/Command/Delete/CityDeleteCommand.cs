using MediatR;
using Domain.Entities;
using Application.Repositories;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Application.Exceptions;

namespace Application.Features.Web.Cities
{
    #region Request
    public class CityDeleteCommand
    : IRequest<BaseResult>, IBaseEntityDTO
    {
        #region Constructors
        public CityDeleteCommand(long id)
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
    public class CityDeleteQueryHandler : IRequestHandler<CityDeleteCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        public CityDeleteQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }


        public async Task<BaseResult> Handle(CityDeleteCommand request, CancellationToken cancellationToken)
        {
            var deletedCities = await _uow.Cities.ExecuteDeleteAsync(x => x.Id == request.Id);
            if (deletedCities == 0)
                throw new NotFoundException("شهر مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
