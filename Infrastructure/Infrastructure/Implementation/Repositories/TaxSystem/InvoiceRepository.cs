using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;
using Application.Features.Web.Invoices;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(DbContext context) : base(context)
        {
        }



        /// <summary>
        /// گرفتن شماره سریال جدید برای صورتحساب
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetNewSerialNumber()
        {
            var lastSerialNumber = await GetOneDTOAsync(
                x => x.SerialNumber,
                x => x.SerialNumber != null,
                orderBy: x => x.OrderByDescending(z => z.SerialNumber)
                );
            var newSerial = (lastSerialNumber ?? 0) + 1;
            return newSerial;
        }


    }
}
