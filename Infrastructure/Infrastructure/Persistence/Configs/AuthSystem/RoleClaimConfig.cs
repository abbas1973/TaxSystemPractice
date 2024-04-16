using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class RoleClaimConfig : BaseEntityConfig<RoleClaim>
    {
        public override void CustomeConfigure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims");

            #region Properties
            builder.Property(x => x.Claim)
                    .IsRequired()
                    .HasMaxLength(300);

            builder.HasOne(x => x.Role)
                .WithMany(y => y.Claims)
                .HasForeignKey(x => x.RoleId);
            #endregion
        }

    }
}
