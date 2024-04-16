using Application.DTOs;
using Application.Repositories;
using Domain.Entities;
using LinqKit;
using MediatR;
using System.Linq.Expressions;

namespace Application.Features.Web.Provinces
{
    #region Request
    public class ProvinceGetSelectListQuery
    : IRequest<List<SelectListDTO>>
    {
        #region Constructors
        public ProvinceGetSelectListQuery()
        {
            JustEnabled = true;
        }
        public ProvinceGetSelectListQuery(bool justEnabled)
        {
            JustEnabled = justEnabled;
        }
        #endregion


        #region Properties
        public bool JustEnabled { get; set; }
        #endregion


        #region توابع
        public Expression<Func<Province, bool>> Getfilter()
        {
            var filter = PredicateBuilder.New<Province>(true);
            if (JustEnabled)
                filter.And(x => x.IsEnabled);

            return filter;
        }
        #endregion
    }
    #endregion



    #region Handler
    public class ProvinceGetSelectListQueryHandler : IRequestHandler<ProvinceGetSelectListQuery, List<SelectListDTO>>
    {
        private readonly IUnitOfWork _uow;
        public ProvinceGetSelectListQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<List<SelectListDTO>> Handle(ProvinceGetSelectListQuery request, CancellationToken cancellationToken)
        {
            var model = await _uow.Provinces.GetDTOAsync(
                SelectListDTO.ProvinceSelector,
                request.Getfilter());

            return model;
        }
    }

    #endregion
}
