using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class CityConfig : BaseEntityConfig<City>
    {
        public override void CustomeConfigure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");

            #region Properties
            builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasOne(x => x.Province)
                .WithMany(y => y.Cities)
                .HasForeignKey(x => x.ProvinceId);
            #endregion
        }

    }
}
