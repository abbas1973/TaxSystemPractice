using Application.DTOs;
using Application.Repositories;
using Domain.Entities;
using LinqKit;
using MediatR;
using System.Linq.Expressions;

namespace Application.Features.Web.Cities
{
    #region Request
    public class CityGetSelectListQuery
    : IRequest<List<SelectListDTO>>
    {
        #region Constructors
        public CityGetSelectListQuery()
        {
            JustEnabled = true;
        }

        public CityGetSelectListQuery(long? provinceId = null, bool justEnabled = true)
        {
            JustEnabled = justEnabled;
            ProvinceId = provinceId;
        }
        #endregion


        #region Properties
        public long? ProvinceId { get; set; }
        public bool JustEnabled { get; set; }
        #endregion


        #region توابع
        public Expression<Func<City, bool>> Getfilter()
        {
            var filter = PredicateBuilder.New<City>(true);
            if (JustEnabled)
                filter.And(x => x.IsEnabled);
            if (ProvinceId != null)
                filter.And(x => x.ProvinceId == ProvinceId);
            return filter;
        }
        #endregion
    }
    #endregion



    #region Handler
    public class CityGetSelectListQueryHandler : IRequestHandler<CityGetSelectListQuery, List<SelectListDTO>>
    {
        private readonly IUnitOfWork _uow;
        public CityGetSelectListQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<List<SelectListDTO>> Handle(CityGetSelectListQuery request, CancellationToken cancellationToken)
        {
            var model = await _uow.Cities.GetDTOAsync(
                SelectListDTO.CitySelector,
                request.Getfilter());

            return model;
        }
    }

    #endregion
}
