using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;

namespace Infrastructure.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(DbContext context) : base(context)
        {
        }

    }
}
