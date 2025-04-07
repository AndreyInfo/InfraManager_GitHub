using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class SupplierConfiguration : SupplierConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_supplier";

        protected override void ConfigureDatabase(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("supplier", Options.Scheme);

            builder.Property(x => x.Address).HasColumnName("address");
            builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.ExternalID).HasColumnName("external_id");
            builder.Property(x => x.Inn).HasColumnName("i_nn");
            builder.Property(x => x.Kpp).HasColumnName("k_pp");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Notice).HasColumnName("notice");
            builder.Property(x => x.Phone).HasColumnName("phone");
            builder.Property(x => x.RegisteredAddress).HasColumnName("registered_address");
            builder.Property(x => x.ID).HasColumnName("supplier_id");
        }
    }
}