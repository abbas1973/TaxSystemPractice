using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;
using LinqKit;

namespace Infrastructure.Repositories
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext context) : base(context)
        {
        }


        #region گرفتن آیدی نقش های کاربر
        /// <summary>
        /// گرفتن آیدی نقش های کاربر
        /// </summary>
        /// <param name="userId">آیدی کاربر</param>
        /// <param name="justEnabled">فقط نقش های فعال لود شود؟</param>
        /// <returns></returns>
        public async Task<List<long>> GetUserRoleIds(long userId, bool justEnabled = true)
        {
            var filter = PredicateBuilder.New<UserRole>();
            filter.Start(x => x.UserId == userId);
            if (justEnabled)
                filter.And(x => x.Role.IsEnabled);
            return GetDTO(x => x.RoleId, filter);
        } 
        #endregion



    }
}
