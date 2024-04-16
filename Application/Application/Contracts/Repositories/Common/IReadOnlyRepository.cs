using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Repositories
{
    /// <summary>
    /// POWERED BY Abbas MohammadNezhad
    /// <para>
    /// email: abbas.mn1973@gmail.com
    /// </para>
    /// </summary>
    public interface IReadOnlyRepository<TEntity>
        where TEntity : IBaseEntity
    {
        #region GetAll
        List<TEntity> GetAll(bool disableTracking = true);
        Task<List<TEntity>> GetAllAsync(bool disableTracking = true);
        #endregion


        #region Get
        List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool disableTracking = true);


        Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool disableTracking = true);
        #endregion


        #region GetDTO
        List<TResult> GetDTO<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool disableTracking = true);

        Task<List<TResult>> GetDTOAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool disableTracking = true);
        #endregion


        #region GetOneDTO
        TResult GetOneDTO<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TResult> GetOneDTOAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        #endregion


        #region Projection With AutoMapper
        #region ProjectToDTO
        /// <summary>
        /// گرفتن لیستی از مدل دلخواه
        /// <para>
        /// _uow.Entities.ProjectTo<EntityDTO>(_mapper.ConfigurationProvider,filter);
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">نوع مدل بازگشتی</typeparam>
        /// <param name="mapperConfig">کانفیگ اتومپر</param>
        /// <param name="filter">شرط ها</param>
        /// <param name="orderBy">مرتب سازی</param>
        /// <param name="skip">تعداد ایتم هایی که باید از آنها عبور شود</param>
        /// <param name="take">تعداد ایتم هایی که باید در خروجی برگردد</param>
        /// <param name="include">وابستگی ها و جوین ها</param>
        /// <returns>لیستی از مدل دلخواه</returns>
        List<TResult> ProjectTo<TResult>(
            IConfigurationProvider mapperConfig = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
            );


        /// <summary>
        /// گرفتن لیستی از مدل دلخواه
        /// به صورت آسنکرون
        /// <para>
        /// _uow.Entities.ProjectToAsync<EntityDTO>(_mapper.ConfigurationProvider,filter);
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">نوع مدل بازگشتی</typeparam>
        /// <param name="mapperConfig">کانفیگ اتومپر</param>
        /// <param name="filter">شرط ها</param>
        /// <param name="orderBy">مرتب سازی</param>
        /// <param name="skip">تعداد ایتم هایی که باید از آنها عبور شود</param>
        /// <param name="take">تعداد ایتم هایی که باید در خروجی برگردد</param>
        /// <param name="include">وابستگی ها و جوین ها</param>
        /// <returns>لیستی از مدل دلخواه</returns>
        Task<List<TResult>> ProjectToAsync<TResult>(
            IConfigurationProvider mapperConfig = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
            );
        #endregion



        #region ProjectToOneDTO
        /// <summary>
        /// گرفتن یک مدل دلخواه
        /// </summary>
        /// <typeparam name="TResult">نوع مدل بازگشتی</typeparam>
        /// <param name="mapperConfig">کانفیگ اتومپر</param>
        /// <param name="filter">شرط ها</param>
        /// <param name="includes">وابستگی ها و جوین ها</param>
        /// <returns>یک مدل دلخواه</returns>
        TResult ProjectToOne<TResult>(
            IConfigurationProvider mapperConfig = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            );



        /// <summary>
        /// گرفتن یک مدل دلخواه بصورت آسنکرون
        /// </summary>
        /// <typeparam name="TResult">نوع مدل بازگشتی</typeparam>
        /// <param name="mapperConfig">کانفیگ اتومپر</param>
        /// <param name="filter">شرط ها</param>
        /// <param name="includes">وابستگی ها و جوین ها</param>
        /// <returns>یک مدل دلخواه</returns>
        Task<TResult> ProjectToOneAsync<TResult>(
            IConfigurationProvider mapperConfig = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            );
        #endregion
        #endregion


        #region SingleOrDefault
        TEntity SingleOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);

        Task<TEntity> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
        #endregion


        #region FirstOrDefault
        TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
        #endregion


        #region GetById
        TEntity GetById(object id);

        Task<TEntity> GetByIdAsync(object id);
        #endregion



        #region Count
        int Count(Expression<Func<TEntity, bool>> filter = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
        #endregion


        #region Sum
        int Sum(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, int>> selector = null);
        decimal Sum(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, decimal>> selector = null);
        float Sum(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, float>> selector = null);
        double Sum(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, double>> selector = null);

        Task<int> SumAsync(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, int>> selector = null);
        Task<decimal> SumAsync(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, decimal>> selector = null);
        Task<float> SumAsync(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, float>> selector = null);
        Task<double> SumAsync(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, double>> selector = null);
        #endregion


        #region Max
        TResult Max<TResult>(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, TResult>> selector = null);

        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, bool>> filter = null,
                               Expression<Func<TEntity, TResult>> selector = null);
        #endregion



        #region Any
        bool Any(Expression<Func<TEntity, bool>> filter = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null); 
        #endregion
    }
}
