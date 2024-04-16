using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Entities;
using MediatR;
using System.ComponentModel;
using Utilities;

namespace Application.Features.Base
{
    #region Request
    [Description("ویرایش آیتم")]
    public abstract class UpdateCommand<TEntity> : BaseEntityDTO, IUpdateCommand<TEntity>
    where TEntity : IBaseEntity
    {
        #region Mapping
        public abstract void Mapping(Profile profile);
        #endregion

    }
    #endregion



    #region Handler
    public abstract class UpdateCommandHandler<TEntity, TCommand> : IRequestHandler<TCommand, BaseResult>
        where TEntity : IBaseEntity
        where TCommand : class, IUpdateCommand<TEntity>
    {
        protected readonly IGenericBaseUnitOfWork<TEntity> _uow;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public UpdateCommandHandler(
            IGenericBaseUnitOfWork<TEntity> uow, 
            IMapper mapper, 
            IMediator mediator)
        {
            _uow = uow;
            _mapper = mapper;
            _mediator = mediator;
        }

        public virtual async Task<BaseResult> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = await _uow.Repository.GetByIdAsync(request.Id);
            if(entity == null)
                throw new NotFoundException("رکورد مورد نظر یافت نشد!");

            entity = _mapper.Map(request, entity);
            _uow.Repository.Update(entity);
            await _uow.CommitAsync();
            await MakeLog(request);
            return new BaseResult(true);
        }

        #region ثبت لاگ عملیات
        protected async Task MakeLog(TCommand request)
        {
            var type = typeof(TEntity);
            var logParams = new List<LogParameterDTO>()
            {
                new LogParameterDTO(
                    $"آیدی {type.GetDescription()}",
                    $"{type.Name}.Id",
                    request.Id),
                new LogParameterDTO(
                    $"اطلاعات ویرایش {type.GetDescription()}",
                    $"{type.Name}UpdateData",
                    request)
            };
            await _mediator.Publish(new LogNotification<TEntity, TCommand>(logParams));

        }
        #endregion

    }
    #endregion
}
