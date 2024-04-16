using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class InvoiceItemConfig : BaseEntityConfig<InvoiceItem>
    {
        public override void CustomeConfigure(EntityTypeBuilder<InvoiceItem> builder)
        {
            builder.ToTable("InvoiceItems");

            #region Properties
            builder.Property(x => x.Name)
                    .HasMaxLength(300);

            builder.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(x => x.CountingUnitName)
                    .HasMaxLength(100);
            #endregion



            #region Relations
            builder.HasOne(x => x.Invoice)
                .WithMany(z => z.InvoiceItems)
                .HasForeignKey(x => x.InvoiceId);

            #endregion
        }

    }
}
