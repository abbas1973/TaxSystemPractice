using Application.Features.Web.Invoices;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {

      

        /// <summary>
        /// گرفتن شماره سریال جدید برای صورتحساب
        /// </summary>
        /// <returns></returns>
        Task<long> GetNewSerialNumber();

    }
}
