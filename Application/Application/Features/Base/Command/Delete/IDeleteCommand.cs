using Application.DTOs;
using Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Application.Features.Base
{
    public interface IDeleteCommand<TEntity> : IRequest<BaseResult> , IBaseEntityDTO
        where TEntity : IBaseEntity
    {

        Expression<Func<TEntity, bool>> GetFilter();
    }
}
