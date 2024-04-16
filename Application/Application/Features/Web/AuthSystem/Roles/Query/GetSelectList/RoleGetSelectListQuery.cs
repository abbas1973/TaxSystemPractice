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

namespace Application.Features.Web.Roles
{
    #region Request
    public class RoleGetSelectListQuery
    : IRequest<List<SelectListDTO>>
    {
        #region Constructors
        public RoleGetSelectListQuery()
        {
            JustEnabled = true;
        }
        public RoleGetSelectListQuery(bool justEnabled)
        {
            JustEnabled = justEnabled;
        }
        #endregion


        #region Properties
        public bool JustEnabled { get; set; }
        #endregion


        #region توابع
        public Expression<Func<Role, bool>> Getfilter()
        {
            var filter = PredicateBuilder.New<Role>(true);
            if (JustEnabled)
                filter.And(x => x.IsEnabled);

            return filter;
        }
        #endregion
    }
    #endregion



    #region Handler
    public class RoleGetSelectListQueryHandler : IRequestHandler<RoleGetSelectListQuery, List<SelectListDTO>>
    {
        private readonly IUnitOfWork _uow;
        public RoleGetSelectListQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<List<SelectListDTO>> Handle(RoleGetSelectListQuery request, CancellationToken cancellationToken)
        {
            var model = await _uow.Roles.GetDTOAsync(
                SelectListDTO.RoleSelector,
                request.Getfilter());

            return model;
        }
    }

    #endregion
}
