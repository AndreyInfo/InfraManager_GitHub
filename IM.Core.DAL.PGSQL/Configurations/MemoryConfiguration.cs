using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class MemoryConfiguration : MemoryConfigurationBase
{
    protected override string PrimaryKeyName => "pk_memory";

    protected override void ConfigureDatabase(EntityTypeBuilder<Memory> builder)
    {
        builder.ToTable("memory", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Capacity).HasColumnName("capacity");
        builder.Property(x => x.DeviceLocator).HasColumnName("device_locator");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
