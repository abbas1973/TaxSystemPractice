using Application.DTOs;
using Base.Application.Mapping;
using Domain.Entities;
using MediatR;

namespace Application.Features.Base
{
    public interface ICreateCommand<TEntity> : IRequest<BaseResult<long>> , IMapFrom<TEntity>
        where TEntity : IBaseEntity {
    
    }

}
