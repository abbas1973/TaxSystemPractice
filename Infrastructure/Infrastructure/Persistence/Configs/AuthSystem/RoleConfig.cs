using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class RoleConfig : BaseEntityConfig<Role>
    {
        public override void CustomeConfigure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            #region Properties
            builder.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(300);

            builder.Property(x => x.Description)
                .HasMaxLength(500);
            #endregion
        }

    }
}
