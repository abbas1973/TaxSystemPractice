using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;

namespace Application.Features.Web.Roles
{
    #region Request
    public class RoleGetListQuery
    : IRequest<DataTableResponseDTO<RoleDataTableDTO>>
    {
        #region Constructors
        public RoleGetListQuery()
        {
        }
        #endregion


        #region Properties
        public string Title { get; set; }
        public bool? IsEnabled { get; set; }
        #endregion


        #region توابع
        public Expression<Func<Role, bool>> GetFilter(string dataTableSearch)
        {
            var filter = PredicateBuilder.New<Role>(true);

            //فعال
            if (IsEnabled != null)
                filter.Start(x => x.IsEnabled == IsEnabled);

            // عنوان
            if (!string.IsNullOrEmpty(Title))
                filter.Start(x => x.Title.Contains(Title));

            //search
            if (!string.IsNullOrEmpty(dataTableSearch))
            {
                var srch = dataTableSearch;
                filter.And(s => s.Title.Contains(dataTableSearch));
            }

            return filter;
        }
        #endregion

    }
    #endregion



    #region Handler
    public class RoleGetListQueryHandler : IRequestHandler<RoleGetListQuery, DataTableResponseDTO<RoleDataTableDTO>>
    {
        private readonly IDataTableManager _dataTableManager;
        private readonly IUnitOfWork _uow;
        public RoleGetListQueryHandler(IUnitOfWork uow, IDataTableManager dataTableManager)
        {
            _uow = uow;
            _dataTableManager = dataTableManager;
        }


        public async Task<DataTableResponseDTO<RoleDataTableDTO>> Handle(RoleGetListQuery request, CancellationToken cancellationToken)
        {
            var model = new DataTableResponseDTO<RoleDataTableDTO>();
            var searchData = _dataTableManager.GetSearchModel();
            var sortCol = $"{searchData.sortColumnName} {searchData.sortDirection}";
            var filter = request.GetFilter(searchData.searchValue);

            model.draw = searchData.draw;
            model.recordsTotal = await _uow.Roles.CountAsync();
            model.recordsFiltered= await _uow.Roles.CountAsync(filter);
            model.data = await _uow.Roles.GetDTOAsync(
                RoleDataTableDTO.Selector,
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
