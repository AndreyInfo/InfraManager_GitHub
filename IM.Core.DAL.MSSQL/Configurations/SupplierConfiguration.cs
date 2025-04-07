using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class SupplierConfiguration : SupplierConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Supplier";

        protected override void ConfigureDatabase(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Supplier", "dbo");

            builder.Property(x => x.Address).HasColumnName("Address");
            builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
            builder.Property(x => x.Email).HasColumnName("Email");
            builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
            builder.Property(x => x.Inn).HasColumnName("INN"); 
            builder.Property(x => x.Kpp).HasColumnName("KPP");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Notice).HasColumnName("Notice");
            builder.Property(x => x.Phone).HasColumnName("Phone");
            builder.Property(x => x.RegisteredAddress).HasColumnName("RegisteredAddress");
            builder.Property(x => x.ID).HasColumnName("SupplierID");
        }
    }
}
