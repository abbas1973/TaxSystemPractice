using Application.DTOs;
using Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Application.Features.Base
{
    public interface IToggleEnableCommand<TEntity> : IRequest<BaseResult> , IBaseEntityDTO
        where TEntity : IBaseEntity, IIsEnabled
    {

        Expression<Func<TEntity, bool>> GetFilter();
    }
}
