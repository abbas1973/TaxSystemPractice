using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        /// <summary>
        /// گرفتن آیدی نقش های کاربر
        /// </summary>
        /// <param name="userId">آیدی کاربر</param>
        /// <param name="justEnabled">فقط نقش های فعال لود شود؟</param>
        /// <returns></returns>
        Task<List<long>> GetUserRoleIds(long userId, bool justEnabled = true);
    }
}
