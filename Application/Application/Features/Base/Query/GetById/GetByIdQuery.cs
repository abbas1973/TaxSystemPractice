using MediatR;
using Application.DTOs;
using System.Linq.Expressions;
using LinqKit;
using Domain.Entities;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Base
{
    #region Request
    public class GetByIdQuery<TEntity, Tout>
    : BaseEntityDTO, IGetByIdQuery<TEntity, Tout>
        where TEntity : IBaseEntity
        where Tout : class
    {
        #region Constructors
        public GetByIdQuery()
        {
            
        }
        public GetByIdQuery(long id)
        {
            Id = id;
        }
        #endregion


        #region توابع
        /// <summary>
        /// گرفتن فیلتر
        /// </summary>
        public Expression<Func<TEntity, bool>> Getfilter()
        {
            var filter = PredicateBuilder.New<TEntity>();
            filter.Start(x => x.Id == Id);
            return filter;
        } 
        #endregion
    }
    #endregion




    #region Handler
    public class GetByIdQueryHandler<TEntity, Tout> : IRequestHandler<IGetByIdQuery<TEntity, Tout>, BaseResult<Tout>>
        where TEntity : IBaseEntity
        where Tout : class
    {
        private readonly IGenericBaseUnitOfWork<TEntity> _uow;
        private readonly IMapper _mapper;
        public GetByIdQueryHandler(
            IGenericBaseUnitOfWork<TEntity> uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult<Tout>> Handle(IGetByIdQuery<TEntity, Tout> request, CancellationToken cancellationToken)
        {
            var model = await _uow.Repository.ProjectToOneAsync<Tout>(
                _mapper.ConfigurationProvider,
                request.Getfilter());
            if (model == null)
                throw new NotFoundException("اطلاعات رکورد مورد نظر یافت نشد!");

            return new BaseResult<Tout>(model);
        }
    }

    #endregion

}
