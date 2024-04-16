using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;

namespace Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

    }
}
