using Domain.Entities;

namespace Application.Repositories
{

    /// <summary>
    /// POWERED BY Abbas MohammadNezhad
    /// <para>
    /// email: abbas.mn1973@gmail.com
    /// </para>
    /// </summary>
    public interface IGenericBaseUnitOfWork<TEntity> : IUnitOfWorkBase
        where TEntity : IBaseEntity
    {
        IRepository<TEntity> Repository { get; }
    }
}
