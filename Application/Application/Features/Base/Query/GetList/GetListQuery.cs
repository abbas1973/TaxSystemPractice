using System.Linq.Expressions;
using Domain.Entities;
using Application.DTOs;

namespace Application.Features.Base
{
    #region Request
    public abstract class GetListQuery<TEntity, Tout>
    : GridSearchDTO, IGetListQuery<TEntity, Tout>
        where TEntity : IBaseEntity
        where Tout : class
    {

        /// <summary>
        /// گرفتن فیلتر
        /// </summary>
        public abstract Expression<Func<TEntity, bool>> GetFilter();

    }
    #endregion
}
