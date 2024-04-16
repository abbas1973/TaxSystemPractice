using Application.DTOs;
using Base.Application.Mapping;
using Domain.Entities;
using MediatR;

namespace Application.Features.Base
{
    public interface IUpdateCommand<TEntity> : IBaseEntityDTO, IRequest<BaseResult>, IMapFrom<TEntity>
        where TEntity : IBaseEntity {
    
    }

}
