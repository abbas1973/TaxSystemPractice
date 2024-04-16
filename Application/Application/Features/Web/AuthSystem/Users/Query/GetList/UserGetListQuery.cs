using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;

namespace Application.Features.Web.Users
{
    #region Request
    public class UserGetListQuery
    : IRequest<DataTableResponseDTO<UserDataTableDTO>>
    {
        #region Constructors
        public UserGetListQuery()
        {
        }
        #endregion


        #region Properties
        public string Name { get; set; }
        public string Mobile { get; set; }
        public long? RoleId { get; set; }
        public bool? IsEnabled { get; set; }
        #endregion


        #region توابع
        public Expression<Func<User, bool>> GetFilter(string dataTableSearch)
        {
            var filter = PredicateBuilder.New<User>(true);

            //فعال
            if (IsEnabled != null)
                filter.Start(x => x.IsEnabled == IsEnabled);

            //نقش
            if (RoleId != null)
                filter.Start(x => x.Roles.Any(r => r.RoleId == RoleId));

            // نام
            if (!string.IsNullOrEmpty(Name))
                filter.Start(x => (x.FirstName + " " + x.LastName).Contains(Name));

            // موبایل
            if (!string.IsNullOrEmpty(Mobile))
                filter.Start(x => x.Mobile.Contains(Name));

            //search
            if (!string.IsNullOrEmpty(dataTableSearch))
            {
                var srch = dataTableSearch;
                filter.And(s => s.Mobile.Contains(dataTableSearch)
                                || (s.FirstName + " " + s.LastName).Contains(dataTableSearch));
            }

            return filter;
        }
        #endregion

    }
    #endregion



    #region Handler
    public class UserGetListQueryHandler : IRequestHandler<UserGetListQuery, DataTableResponseDTO<UserDataTableDTO>>
    {
        private readonly IDataTableManager _dataTableManager;
        private readonly IUnitOfWork _uow;
        public UserGetListQueryHandler(IUnitOfWork uow, IDataTableManager dataTableManager)
        {
            _uow = uow;
            _dataTableManager = dataTableManager;
        }


        public async Task<DataTableResponseDTO<UserDataTableDTO>> Handle(UserGetListQuery request, CancellationToken cancellationToken)
        {
            var model = new DataTableResponseDTO<UserDataTableDTO>();
            var searchData = _dataTableManager.GetSearchModel();
            var sortCol = $"{searchData.sortColumnName} {searchData.sortDirection}";
            var filter = request.GetFilter(searchData.searchValue);

            model.draw = searchData.draw;
            model.recordsTotal = await _uow.Users.CountAsync();
            model.recordsFiltered= await _uow.Users.CountAsync(filter);
            model.data = await _uow.Users.GetDTOAsync(
                UserDataTableDTO.Selector,
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
