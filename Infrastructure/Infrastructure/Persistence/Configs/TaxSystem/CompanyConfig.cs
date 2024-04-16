using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class CompanyConfig : BaseEntityConfig<Company>
    {
        public override void CustomeConfigure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            #region Properties
            builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(300);

            builder.Property(x => x.PrivateKey)
                    .IsRequired()
                    .HasMaxLength(3000);

            builder.Property(x => x.ClientId)
                    .IsRequired()
                    .HasMaxLength(20);

            builder.Property(x => x.EconomicCode)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(x => x.NationalCode)
                    //.IsRequired()
                    .HasMaxLength(11);

            #endregion

        }

    }
}
