using MediatR;
using Application.DTOs;
using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Features.Base
{
    #region Request
    public interface IGetByIdQuery<TEntity, Tout>
    : IBaseEntityDTO, IRequest<BaseResult<Tout>>
        where TEntity : IBaseEntity
        where Tout : class
    {

        Expression<Func<TEntity, bool>> Getfilter();
    }
    #endregion



}
