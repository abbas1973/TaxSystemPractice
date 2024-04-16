using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;

namespace Infrastructure.Repositories
{
    public class RoleClaimRepository : Repository<RoleClaim>, IRoleClaimRepository
    {
        public RoleClaimRepository(DbContext context) : base(context)
        {
        }


        /// <summary>
        /// گرفتن دسترسی های یک نقش
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetRolePermissions(long roleId)
        {
            return await GetDTOAsync(x => x.Claim, x => x.RoleId == roleId);
        }


        /// <summary>
        /// گرفتن دسترسی های نقش ها
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetRolePermissions(List<long> roleIds)
        {
            return await GetDTOAsync(x => x.Claim, x => roleIds.Contains(x.RoleId));
        }

    }
}
