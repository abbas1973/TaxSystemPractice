namespace Application.Repositories
{

    /// <summary>
    /// POWERED BY Abbas MohammadNezhad
    /// <para>
    /// email: abbas.mn1973@gmail.com
    /// </para>
    /// </summary>
    public interface IUnitOfWorkBase : IDisposable
    {
        #region توابع عمومی
        void Commit();

        Task CommitAsync();

        bool ExecuteNonQuery(string Query, params object[] Parameters);

        Task<bool> ExecuteNonQueryAsync(string Query, params object[] Parameters); 
        #endregion
    }
}
