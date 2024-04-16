using MediatR;
using System.Linq.Expressions;
using Domain.Entities;
using Application.DTOs;

namespace Application.Features.Base
{
    public interface IGetListQuery<TEntity, Tout>
    : IGridSearchDTO, IRequest<BaseResult<ItemGridDTO<Tout>>>
        where TEntity : IBaseEntity
        where Tout : class
    {

        /// <summary>
        /// گرفتن فیلتر
        /// </summary>
        Expression<Func<TEntity, bool>> GetFilter();

    }
}
