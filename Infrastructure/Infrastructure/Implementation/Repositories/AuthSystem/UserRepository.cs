using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

    }
}
