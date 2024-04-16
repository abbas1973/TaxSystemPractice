using MediatR;
using Application.Repositories;
using System.ComponentModel.DataAnnotations;
using Redis.Services;
using StackExchange.Redis;

namespace Application.Features.Web.Users
{
    #region Request
    public class UserGetClaimsQuery
    : IRequest<List<string>>
    {
        #region Constructors
        public UserGetClaimsQuery(long userId)
        {
            UserId = userId;
        }
        #endregion


        #region Properties
        [Display(Name = "شناسه کاربر")]
        public long UserId { get; set; }
        #endregion

    }
    #endregion



    #region Handler
    public class UserGetClaimsQueryHandler : IRequestHandler<UserGetClaimsQuery, List<string>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRedisManager _redis;
        public UserGetClaimsQueryHandler(IUnitOfWork uow, IRedisManager redis)
        {
            _uow = uow;
            _redis = redis;
        }


        public async Task<List<string>> Handle(UserGetClaimsQuery request, CancellationToken cancellationToken)
        {
            #region گرفتن اطلاعات از ردیس
            var claims = await _redis.db.GetUserClaims(request.UserId);
            if (claims != null && claims.Any())
                return claims;
            #endregion

            #region گرفتن نقش های کاربر
            var roleIds = await _uow.UserRoles.GetUserRoleIds(request.UserId);
            if (roleIds == null || !roleIds.Any())
                return new List<string>();
            #endregion

            #region گرفتن کلایم های نقش ها
            claims = await _uow.RoleClaims.GetDTOAsync(
                    x => x.Claim,
                    x => roleIds.Contains(x.RoleId));
            #endregion

            #region ذخیره اطلاعات در ردیس
            _ = await _redis.db.SetUserClaims(request.UserId, claims);
            #endregion

            return claims;
        }
    }

    #endregion
}
