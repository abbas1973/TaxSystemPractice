namespace Application.Repositories
{

    /// <summary>
    /// POWERED BY Abbas MohammadNezhad
    /// <para>
    /// email: abbas.mn1973@gmail.com
    /// </para>
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkBase, IDisposable
    {
        #region Auth System
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IUserRoleRepository UserRoles { get; }
        IRoleClaimRepository RoleClaims { get; }
        #endregion


        #region Shared
        ICityRepository Cities { get; }
        IProvinceRepository Provinces { get; }
        #endregion


        #region TaxSystem
        ICompanyRepository Companies { get; }
        IInvoiceRepository Invoices { get; }
        IInvoiceItemRepository InvoiceItems { get; }
        #endregion


    }
}
