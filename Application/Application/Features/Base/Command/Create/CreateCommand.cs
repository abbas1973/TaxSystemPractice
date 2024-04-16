using Application.DTOs;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.ComponentModel;
using Utilities;

namespace Application.Features.Base
{
    #region Request
    [Description("افزودن آیتم")]
    public abstract class CreateCommand<TEntity> : ICreateCommand<TEntity>
    where TEntity : IBaseEntity
    {


        #region Mapping
        public abstract void Mapping(Profile profile);
        #endregion
    }
    #endregion



    #region Handler
    public abstract class CreateCommandHandler<TEntity, TCommand> : IRequestHandler<TCommand, BaseResult<long>>
        where TEntity : IBaseEntity
        where TCommand : class, ICreateCommand<TEntity>
    {
        protected readonly IGenericBaseUnitOfWork<TEntity> _uow;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public CreateCommandHandler(
            IGenericBaseUnitOfWork<TEntity> uow, 
            IMapper mapper, 
            IMediator mediator)
        {
            _uow = uow;
            _mapper = mapper;
            _mediator = mediator;
        }

        public virtual async Task<BaseResult<long>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(request);
            await _uow.Repository.AddAsync(entity);
            await _uow.CommitAsync();
            await MakeLog(request);
            return new BaseResult<long>(entity.Id);
        }


        #region ثبت لاگ عملیات
        protected async Task MakeLog(TCommand request)
        {
            var logParams = new List<LogParameterDTO>()
            {
                new LogParameterDTO(
                    $"اطلاعات افزودن {typeof(TEntity).GetDescription()}",
                    $"{typeof(TEntity).Name}CreateData",
                    request)
            };
            await _mediator.Publish(new LogNotification<TEntity, TCommand>(logParams));

        }
        #endregion
    }
    #endregion
}
