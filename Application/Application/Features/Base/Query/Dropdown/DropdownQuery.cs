using Application.DTOs;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using LinqKit;
using MediatR;
using System.Linq.Expressions;

namespace Application.Features.Base
{
    #region Request
    public class DropdownQuery<TEntity>
    : IDropdownQuery<TEntity>
        where TEntity : IBaseEntity , new()
    {
        #region Constructors
        public DropdownQuery()
        {
        }
        #endregion


        #region Properties
        public bool JustEnabled { get; set; } = true;
        #endregion


        #region توابع
        public virtual Expression<Func<TEntity, bool>> GetFilter()
        {
            var filter = PredicateBuilder.New<TEntity>(true);

            if (JustEnabled && typeof(IIsEnabled).IsAssignableFrom(typeof(TEntity)))
                filter.And(x => ((IIsEnabled)x).IsEnabled == true);

            return filter;
        } 
        #endregion
    }
    #endregion




    #region Handler
    public class DropdownQueryHandler<TEntity> : IRequestHandler<IDropdownQuery<TEntity>, BaseResult<List<SelectListDTO>>>
        where TEntity : IBaseEntity
    {
        private readonly IGenericBaseUnitOfWork<TEntity> _uow;
        private readonly IMapper _mapper;
        public DropdownQueryHandler(
            IGenericBaseUnitOfWork<TEntity> uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult<List<SelectListDTO>>> Handle(IDropdownQuery<TEntity> request, CancellationToken cancellationToken)
        {
            var model = await _uow.Repository.ProjectToAsync<SelectListDTO>(
                _mapper.ConfigurationProvider,
                request.GetFilter(),
                take: 100);
            return new BaseResult<List<SelectListDTO>>(model);
        }
    }

    #endregion
}
