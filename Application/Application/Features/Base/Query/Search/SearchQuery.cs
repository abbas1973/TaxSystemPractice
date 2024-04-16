using MediatR;
using System.Linq.Expressions;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using LinqKit;
using Utilities;
using Application.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Application.Features.Base
{
    #region Request
    public class SearchQuery<TEntity>
    : ISearchQuery<TEntity>
     where TEntity : IBaseEntity
    {
        #region Constructors
        public SearchQuery()
        {
        }
        #endregion


        #region Properties
        /// <summary>
        /// متن جستجو
        /// </summary>
        [Display(Name = "متن جستجو")]
        public string Text { get; set; }

        /// <summary>
        /// فقط رکورد های فعال
        /// </summary>
        [Display(Name = "فقط رکورد های فعال")]
        public bool JustEnabled { get; set; } = true;

        /// <summary>
        /// تعداد آیتم های بازگشتی. باید بین 0 تا 100 باشد
        /// </summary>
        [Display(Name = "تعداد درخواستی")]
        public int Count { get; set; } = 20;
        #endregion


        #region توابع
        public virtual Expression<Func<TEntity, bool>> GetFilter()
        {
            var filter = PredicateBuilder.New<TEntity>(true);

            if (JustEnabled && typeof(IIsEnabled).IsAssignableFrom(typeof(TEntity)))
                filter.And(x => ((IIsEnabled)x).IsEnabled == true);


            if (!string.IsNullOrEmpty(Text))
            {
                // اگر انتیتی فیلدی با عنوان Title داشت در آن سرچ شود
                if (typeof(TEntity).HasProperty("Title"))
                    filter.And(x => EF.Property<string>(x, "Title").Contains(Text));

                // اگر انتیتی فیلدی با عنوان Name داشت در آن سرچ شود
                if (typeof(TEntity).HasProperty("Name"))
                    filter.And(x => EF.Property<string>(x, "Name").Contains(Text));
            }

            return filter;
        }
        #endregion

    }
    #endregion




    #region Handler
    public class SearchQueryHandler<TEntity> : IRequestHandler<ISearchQuery<TEntity>, List<SelectListDTO>>
     where TEntity : IBaseEntity
    {
        private readonly IGenericBaseUnitOfWork<TEntity> _uow;
        private readonly IMapper _mapper;
        public SearchQueryHandler(
            IGenericBaseUnitOfWork<TEntity> uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<List<SelectListDTO>> Handle(ISearchQuery<TEntity> request, CancellationToken cancellationToken)
        {
            var model = await _uow.Repository.ProjectToAsync<SelectListDTO>(
                _mapper.ConfigurationProvider,
                request.GetFilter(),
                take: request.Count);
            return model;
        }
    }

    #endregion
}
