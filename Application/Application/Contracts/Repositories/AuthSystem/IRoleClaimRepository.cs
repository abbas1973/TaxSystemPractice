using Domain.Entities;

namespace Application.Repositories
{
    public interface IRoleClaimRepository : IRepository<RoleClaim>
    {

        /// <summary>
        /// گرفتن دسترسی های یک نقش
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetRolePermissions(long roleId);


        /// <summary>
        /// گرفتن دسترسی های نقش ها
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetRolePermissions(List<long> roleIds);
    }
}
