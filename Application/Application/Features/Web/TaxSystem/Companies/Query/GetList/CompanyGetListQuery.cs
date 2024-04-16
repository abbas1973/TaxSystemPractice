using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Companies
{
    #region Request
    public class CompanyGetListQuery
    : IRequest<DataTableResponseDTO<CompanyDataTableDTO>>
    {
        #region Constructors
        public CompanyGetListQuery()
        {
        }
        #endregion


        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }


        /// <summary>
        /// شناسه اقتصادی شرکت
        /// </summary>
        [Display(Name = "شناسه اقتصادی")]
        public string EconomicCode { get; set; }

        [Display(Name = "شناسه ملی")]
        public string NationalCode { get; set; }

        /// <summary>
        /// کلاینت آیدی از سامانه مودیان
        /// </summary>
        [Display(Name = "کلاینت آیدی سامانه مودیان")]
        public string ClientId { get; set; }
        #endregion


        #region توابع
        public Expression<Func<Company, bool>> GetFilter(string dataTableSearch)
        {
            var filter = PredicateBuilder.New<Company>(true);

            //شماره اقتصادی
            if (EconomicCode != null)
                filter.And(x => x.EconomicCode == EconomicCode);

            if (NationalCode != null)
                filter.And(x => x.NationalCode == NationalCode);


            if (ClientId != null)
                filter.And(x => x.ClientId == ClientId);

            // نام
            if (!string.IsNullOrEmpty(Name))
                filter.And(x => x.Name.Contains(Name));

            //search
            if (!string.IsNullOrEmpty(dataTableSearch))
            {
                var srch = dataTableSearch;
                filter.And(s => s.Name.Contains(dataTableSearch) || s.EconomicCode.Contains(dataTableSearch));
            }

            return filter;
        }
        #endregion

    }
    #endregion



    #region Handler
    public class CompanyGetListQueryHandler : IRequestHandler<CompanyGetListQuery, DataTableResponseDTO<CompanyDataTableDTO>>
    {
        private readonly IDataTableManager _dataTableManager;
        private readonly IUnitOfWork _uow;
        public CompanyGetListQueryHandler(IUnitOfWork uow, IDataTableManager dataTableManager)
        {
            _uow = uow;
            _dataTableManager = dataTableManager;
        }


        public async Task<DataTableResponseDTO<CompanyDataTableDTO>> Handle(CompanyGetListQuery request, CancellationToken cancellationToken)
        {
            var model = new DataTableResponseDTO<CompanyDataTableDTO>();
            var searchData = _dataTableManager.GetSearchModel();
            var sortCol = $"{searchData.sortColumnName} {searchData.sortDirection}";
            var filter = request.GetFilter(searchData.searchValue);

            model.draw = searchData.draw;
            model.recordsTotal = await _uow.Companies.CountAsync();
            model.recordsFiltered= await _uow.Companies.CountAsync(filter);
            model.data = await _uow.Companies.GetDTOAsync(
                CompanyDataTableDTO.Selector,
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
