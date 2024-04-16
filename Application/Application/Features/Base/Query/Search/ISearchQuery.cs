using MediatR;
using System.Linq.Expressions;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Features.Base
{
    public interface ISearchQuery<TEntity>
    : IRequest<List<SelectListDTO>>
        where TEntity : IBaseEntity
    {


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
        public bool JustEnabled { get; set; }

        /// <summary>
        /// تعداد آیتم های بازگشتی. باید بین 0 تا 100 باشد
        /// </summary>
        [Display(Name = "تعداد درخواستی")]
        public int Count { get; set; }
        #endregion



        Expression<Func<TEntity, bool>> GetFilter();

    }


}
