using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;

namespace Application.Features.Web.Cities
{
    #region Request
    public class CityGetListQuery
    : IRequest<DataTableResponseDTO<CityDataTableDTO>>
    {
        #region Constructors
        public CityGetListQuery()
        {
        }
        #endregion


        #region Properties
        public string Name { get; set; }
        public string Privince { get; set; }
        public bool? IsEnabled { get; set; }
        #endregion


        #region توابع
        public Expression<Func<City, bool>> GetFilter(string dataTableSearch)
        {
            var filter = PredicateBuilder.New<City>(true);

            //فعال
            if (IsEnabled != null)
                filter.Start(x => x.IsEnabled == IsEnabled);

            // نام
            if (!string.IsNullOrEmpty(Name))
                filter.Start(x => x.Name.Contains(Name));

            //search
            if (!string.IsNullOrEmpty(dataTableSearch))
            {
                var srch = dataTableSearch;
                filter.And(s => s.Name.Contains(dataTableSearch)
                                || s.Province.Name.Contains(dataTableSearch));
            }

            return filter;
        }
        #endregion

    }
    #endregion



    #region Handler
    public class CityGetListQueryHandler : IRequestHandler<CityGetListQuery, DataTableResponseDTO<CityDataTableDTO>>
    {
        private readonly IDataTableManager _dataTableManager;
        private readonly IUnitOfWork _uow;
        public CityGetListQueryHandler(IUnitOfWork uow, IDataTableManager dataTableManager)
        {
            _uow = uow;
            _dataTableManager = dataTableManager;
        }


        public async Task<DataTableResponseDTO<CityDataTableDTO>> Handle(CityGetListQuery request, CancellationToken cancellationToken)
        {
            var model = new DataTableResponseDTO<CityDataTableDTO>();
            var searchData = _dataTableManager.GetSearchModel();
            var sortCol = $"{searchData.sortColumnName} {searchData.sortDirection}";
            var filter = request.GetFilter(searchData.searchValue);

            model.draw = searchData.draw;
            model.recordsTotal = await _uow.Cities.CountAsync();
            model.recordsFiltered= await _uow.Cities.CountAsync(filter);
            model.data = await _uow.Cities.GetDTOAsync(
                CityDataTableDTO.Selector,
                filter,
                x => x.OrderBy(sortCol),
                searchData.start,
                searchData.length
                );

            return model;
        }
    }

    #endregion
}
