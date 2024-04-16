using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Cities
{
    #region Request
    public class CityGetUpdateDTOQuery
    : IRequest<CityUpdateDTO>, IBaseEntityDTO
    {
        #region Constructors
        public CityGetUpdateDTOQuery(long id)
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
    public class CityGetUpdateDTOQueryHandler : IRequestHandler<CityGetUpdateDTOQuery, CityUpdateDTO>
    {
        private readonly IUnitOfWork _uow;
        public CityGetUpdateDTOQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<CityUpdateDTO> Handle(CityGetUpdateDTOQuery request, CancellationToken cancellationToken)
        {
            var city = await _uow.Cities.GetOneDTOAsync(
                CityUpdateDTO.Selector,
                x => x.Id == request.Id);
            if (city == null)
                throw new NotFoundException("اطلاعات شهر مورد نظر یافت نشد!");

            return city;
        }
    }

    #endregion
}
