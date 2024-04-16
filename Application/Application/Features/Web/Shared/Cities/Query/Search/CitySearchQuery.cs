using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;
using Application.DTOs;
using Azure.Core;

namespace Application.Features.Web.Cities
{
    #region Request
    public class CitySearchQuery
    : IRequest<List<SelectListDTO>>
    {
        #region Constructors
        public CitySearchQuery()
        {
            JustEnabled = true;
        }
        public CitySearchQuery(string text = null, long? provinceId = null, bool justEnabled = true, int count = 20)
        {
            Text = text;
            JustEnabled = justEnabled;
            ProvinceId = provinceId;
            Count = count;
        }
        #endregion


        #region Properties
        public string Text { get; set; }
        public long? ProvinceId { get; set; }
        public bool JustEnabled { get; set; }
        public int Count { get; set; }
        #endregion


        #region توابع
        public Expression<Func<City, bool>> Getfilter()
        {
            var filter = PredicateBuilder.New<City>(true);

            if (JustEnabled)
                filter.And(x => x.IsEnabled);

            if(ProvinceId != null)
                filter.And(x => x.ProvinceId == ProvinceId);

            if (!string.IsNullOrWhiteSpace(Text))
                filter.And(x => x.Name == Text);
            return filter;
        }
        #endregion
    }
    #endregion



    #region Handler
    public class CitySearchQueryHandler : IRequestHandler<CitySearchQuery, List<SelectListDTO>>
    {
        private readonly IUnitOfWork _uow;
        public CitySearchQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<List<SelectListDTO>> Handle(CitySearchQuery request, CancellationToken cancellationToken)
        {
            var model = await _uow.Cities.GetDTOAsync(
                SelectListDTO.CitySelector,
                request.Getfilter(),
                take: request.Count);

            return model;
        }
    }

    #endregion
}
