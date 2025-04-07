using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class StorageControllerConfiguration : StorageControllerConfigurationBase
{
    protected override string PrimaryKeyName => "pk_storage_controller";

    protected override void ConfigureDatabase(EntityTypeBuilder<StorageController> builder)
    {
        builder.ToTable("storage_controller", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.WWn).HasColumnName("w_wn");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
