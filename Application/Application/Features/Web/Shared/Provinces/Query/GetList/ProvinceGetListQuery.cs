using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;

namespace Application.Features.Web.Provinces
{
    #region Request
    public class ProvinceGetListQuery
    : IRequest<DataTableResponseDTO<ProvinceDataTableDTO>>
    {
        #region Constructors
        public ProvinceGetListQuery()
        {
        }
        #endregion


        #region Properties
        public string Name { get; set; }
        public bool? IsEnabled { get; set; }
        #endregion


        #region توابع
        public Expression<Func<Province, bool>> GetFilter(string dataTableSearch)
        {
            var filter = PredicateBuilder.New<Province>(true);

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
                filter.And(s => s.Name.Contains(dataTableSearch));
            }

            return filter;
        }
        #endregion

    }
    #endregion



    #region Handler
    public class ProvinceGetListQueryHandler : IRequestHandler<ProvinceGetListQuery, DataTableResponseDTO<ProvinceDataTableDTO>>
    {
        private readonly IDataTableManager _dataTableManager;
        private readonly IUnitOfWork _uow;
        public ProvinceGetListQueryHandler(IUnitOfWork uow, IDataTableManager dataTableManager)
        {
            _uow = uow;
            _dataTableManager = dataTableManager;
        }


        public async Task<DataTableResponseDTO<ProvinceDataTableDTO>> Handle(ProvinceGetListQuery request, CancellationToken cancellationToken)
        {
            var model = new DataTableResponseDTO<ProvinceDataTableDTO>();
            var searchData = _dataTableManager.GetSearchModel();
            var sortCol = $"{searchData.sortColumnName} {searchData.sortDirection}";
            var filter = request.GetFilter(searchData.searchValue);

            model.draw = searchData.draw;
            model.recordsTotal = await _uow.Provinces.CountAsync();
            model.recordsFiltered= await _uow.Provinces.CountAsync(filter);
            model.data = await _uow.Provinces.GetDTOAsync(
                ProvinceDataTableDTO.Selector,
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
