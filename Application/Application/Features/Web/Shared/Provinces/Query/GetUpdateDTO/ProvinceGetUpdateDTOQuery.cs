using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Provinces
{
    #region Request
    public class ProvinceGetUpdateDTOQuery
    : IRequest<ProvinceUpdateDTO>, IBaseEntityDTO
    {
        #region Constructors
        public ProvinceGetUpdateDTOQuery(long id)
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
    public class ProvinceGetUpdateDTOQueryHandler : IRequestHandler<ProvinceGetUpdateDTOQuery, ProvinceUpdateDTO>
    {
        private readonly IUnitOfWork _uow;
        public ProvinceGetUpdateDTOQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<ProvinceUpdateDTO> Handle(ProvinceGetUpdateDTOQuery request, CancellationToken cancellationToken)
        {
            var province = await _uow.Provinces.GetOneDTOAsync(
                ProvinceUpdateDTO.Selector,
                x => x.Id == request.Id);
            if (province == null)
                throw new NotFoundException("اطلاعات استان مورد نظر یافت نشد!");

            return province;
        }
    }

    #endregion
}
