using MediatR;
using Domain.Entities;
using Application.Repositories;
using Application.DTOs;
using Application.Exceptions;
using LinqKit;
using System.Linq.Expressions;
using Utilities;
using System.ComponentModel;

namespace Application.Features.Base
{
    #region Request
    [Description("تغییر وضعیت فعال/غیرفعال")]
    public class ToggleEnableCommand<TEntity>
        : BaseEntityDTO, IToggleEnableCommand<TEntity>
        where TEntity: IBaseEntity, IIsEnabled
    {
        #region Constructors
        public ToggleEnableCommand()
        {
            
        }
        public ToggleEnableCommand(long id)
        {
            Id = id;
        }
        #endregion



        #region توابع
        /// <summary>
        /// گرفتن فیلتر
        /// </summary>
        public virtual Expression<Func<TEntity, bool>> GetFilter()
        {
            var filter = PredicateBuilder.New<TEntity>();
            filter.Start(x => x.Id == Id);
            return filter;
        } 
        #endregion

    }
    #endregion



    #region Handler
    public class ToggleEnableCommandHandler<TEntity> : IRequestHandler<IToggleEnableCommand<TEntity>, BaseResult>
    where TEntity : IBaseEntity, IIsEnabled
    {
        protected readonly IGenericBaseUnitOfWork<TEntity> _uow;
        protected readonly IMediator _mediator;
        public ToggleEnableCommandHandler(
            IGenericBaseUnitOfWork<TEntity> uow,
            IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }


        public virtual async Task<BaseResult> Handle(IToggleEnableCommand<TEntity> request, CancellationToken cancellationToken)
        {
            var res = await _uow.Repository.ExecuteUpdateAsync(
                request.GetFilter(),
                x => x.SetProperty(z => z.IsEnabled, z => !z.IsEnabled));
            if (res == 0)
                throw new NotFoundException("رکورد مورد نظر یافت نشد!");
            await MakeLog(request.Id);
            return new BaseResult(true);
        }

        #region ثبت لاگ عملیات
        private async Task MakeLog(long id)
        {
            var type = typeof(TEntity);
            var logParams = new List<LogParameterDTO>()
            {
                new LogParameterDTO(
                    $"آیدی {type.GetDescription()}",
                    $"{type.Name}.Id",
                    id)
            };
            await _mediator.Publish(new LogNotification<TEntity, ToggleEnableCommand<TEntity>>(logParams));

        }
        #endregion
    }

    #endregion
}
