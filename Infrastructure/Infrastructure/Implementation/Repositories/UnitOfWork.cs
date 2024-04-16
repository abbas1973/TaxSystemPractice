using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Application.Repositories;

namespace Infrastructure.Repositories
{

    /// <summary>
    /// POWERED BY Abbas MohammadNezhad
    /// <para>
    /// email: abbas.mn1973@gmail.com
    /// </para>
    /// </summary>
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(DbContext context) : base(context)
        {
        }


        #region Repositories
        #region AuthSystem
        #region User 
        private IUserRepository _users;
        public IUserRepository Users
        {
            get
            {
                if (_users == null)
                    _users = new UserRepository(_context);
                return _users;
            }
        }
        #endregion


        #region Role 
        private IRoleRepository _roles;
        public IRoleRepository Roles
        {
            get
            {
                if (_roles == null)
                    _roles = new RoleRepository(_context);
                return _roles;
            }
        }
        #endregion


        #region UserRole 
        private IUserRoleRepository _userRoles;
        public IUserRoleRepository UserRoles
        {
            get
            {
                if (_userRoles == null)
                    _userRoles = new UserRoleRepository(_context);
                return _userRoles;
            }
        }
        #endregion


        #region RoleClaim 
        private IRoleClaimRepository _roleClaims;
        public IRoleClaimRepository RoleClaims
        {
            get
            {
                if (_roleClaims == null)
                    _roleClaims = new RoleClaimRepository(_context);
                return _roleClaims;
            }
        }
        #endregion
        #endregion


        #region Shared
        #region Province 
        private IProvinceRepository _provinces;
        public IProvinceRepository Provinces
        {
            get
            {
                if (_provinces == null)
                    _provinces = new ProvinceRepository(_context);
                return _provinces;
            }
        }
        #endregion


        #region City 
        private ICityRepository _cities;
        public ICityRepository Cities
        {
            get
            {
                if (_cities == null)
                    _cities = new CityRepository(_context);
                return _cities;
            }
        }
        #endregion
        #endregion


        #region TaxSystem
        #region Company 
        private ICompanyRepository _companies;
        public ICompanyRepository Companies
        {
            get
            {
                if (_companies == null)
                    _companies = new CompanyRepository(_context);
                return _companies;
            }
        }
        #endregion

        #region Invoice 
        private IInvoiceRepository _invoices;
        public IInvoiceRepository Invoices
        {
            get
            {
                if (_invoices == null)
                    _invoices = new InvoiceRepository(_context);
                return _invoices;
            }
        }
        #endregion


        #region InvoiceItem 
        private IInvoiceItemRepository _invoiceItems;
        public IInvoiceItemRepository InvoiceItems
        {
            get
            {
                if (_invoiceItems == null)
                    _invoiceItems = new InvoiceItemRepository(_context);
                return _invoiceItems;
            }
        }
        #endregion
        #endregion
        #endregion




    }
}
