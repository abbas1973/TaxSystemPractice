using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;

namespace Infrastructure.Repositories
{
    public class InvoiceItemRepository : Repository<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(DbContext context) : base(context)
        {
        }

    }
}
