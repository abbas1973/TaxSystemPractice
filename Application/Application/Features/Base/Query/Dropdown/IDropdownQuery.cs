using MediatR;
using Application.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Features.Base
{
    #region Request
    public interface IDropdownQuery<TEntity>
    : IRequest<BaseResult<List<SelectListDTO>>>
    where TEntity : IBaseEntity
    {

        Expression<Func<TEntity, bool>> GetFilter();
    }
    #endregion



}
