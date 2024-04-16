using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;

namespace Infrastructure.Repositories
{
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        public ProvinceRepository(DbContext context) : base(context)
        {
        }

    }
}
