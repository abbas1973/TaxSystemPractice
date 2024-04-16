using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class UserRoleConfig : BaseEntityConfig<UserRole>
    {
        public override void CustomeConfigure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            #region Properties
            builder.HasOne(x => x.User)
                .WithMany(y => y.Roles)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Role)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.RoleId);
            #endregion
        }

    }
}
