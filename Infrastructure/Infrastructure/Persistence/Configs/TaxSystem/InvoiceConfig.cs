using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Infrastructure.Persistence
{
    public class InvoiceConfig : BaseEntityConfig<Invoice>
    {
        public override void CustomeConfigure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            #region Properties
            builder.Property(x => x.InvoiceNumber)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(x => x.BuyerName)
                    .IsRequired()
                    .HasMaxLength(300);

            builder.Property(x => x.BuyerNationalCode)
                    .IsRequired()
                    .HasMaxLength(11);

            builder.Property(x => x.BuyerEconomicCode)
                    .HasMaxLength(50);

            builder.Property(x => x.BuyerMobile)
                    .HasMaxLength(11);

            builder.Property(x => x.BuyerAddress)
                    .HasMaxLength(1500);

            builder.Property(x => x.BuyerPostalCode)
                    .HasMaxLength(10);

            builder.Property(x => x.BuyerPhone)
                    .HasMaxLength(11);

            builder.Property(x => x.Description)
                    .HasMaxLength(3000);


            builder.Property(x => x.ContractId)
                    .HasMaxLength(12);


            builder.Property(x => x.TaxId)
                    .HasMaxLength(22);

            builder.Property(x => x.TaxUid)
                    .HasMaxLength(100);

            builder.Property(x => x.TaxRefNumber)
                    .HasMaxLength(100);

            builder.Property(x => x.TaxErrorCode)
                    .HasMaxLength(100);

            builder.Property(x => x.TaxErrorDetail)
                    .HasMaxLength(100);


            builder.Property(x => x.TaxStatus)
                    .HasMaxLength(50);

            builder.Property(x => x.TaxStatusMessage)
                    .HasMaxLength(500);

            builder.Property(x => x.TaxPacketType)
                    .HasMaxLength(100);

            builder.Property(x => x.TaxFiscalId)
                    .HasMaxLength(100);

            builder.Property(x => x.TaxInquiryData)
                    .HasMaxLength(5000);

            //builder.Property(x => x.EconomicCodeSeller)
            //        .HasMaxLength(50);
            #endregion



            #region Relations
            builder.HasOne(x => x.Company)
                .WithMany(z => z.Invoices)
                .HasForeignKey(x => x.CompanyId);

            #endregion
        }

    }
}
