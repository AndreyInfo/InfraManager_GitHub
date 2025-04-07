using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class MonitorConfiguration : MonitorConfigurationBase
{
    protected override string PrimaryKeyName => "pk_monitor";

    protected override void ConfigureDatabase(EntityTypeBuilder<Monitor> builder)
    {
        builder.ToTable("monitor", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Resolution).HasColumnName("resolution");
        builder.Property(x => x.FontResolution).HasColumnName("font_resolution");
        builder.Property(x => x.Diagonal).HasColumnName("diagonal");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
