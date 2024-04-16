using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class UserConfig : BaseEntityConfig<User> //IEntityTypeConfiguration<User>
    {
        public override void CustomeConfigure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            #region Relations
            builder.HasOne(x => x.Company)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.CompanyId);


            builder.HasOne(x => x.City)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.CityId);
            #endregion

            #region Properties
            builder.Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Mobile)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.CityId)
                .IsRequired();
            #endregion
        }

    }
}
