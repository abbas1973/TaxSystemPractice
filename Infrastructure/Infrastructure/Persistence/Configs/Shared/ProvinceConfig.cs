using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class ProvinceConfig : BaseEntityConfig<Province>
    {
        public override void CustomeConfigure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces");

            #region Properties
            builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            #endregion
        }

    }
}
