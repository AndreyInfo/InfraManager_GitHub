using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public partial class ProcessorConfiguration : ProcessorConfigurationBase
{
    protected override string PrimaryKeyName => "pk_processor";

    protected override void ConfigureDatabase(EntityTypeBuilder<Processor> builder)
    {
        builder.ToTable("processor", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.MaxClockSpeed).HasColumnName("max_clock_speed");
        builder.Property(x => x.CurrentClockSpeed).HasColumnName("current_clock_speed");
        builder.Property(x => x.L2cacheSize).HasColumnName("l2_cache_size");
        builder.Property(x => x.SocketDesignation).HasColumnName("socket_designation");
        builder.Property(x => x.Platform).HasColumnName("platform");
        builder.Property(x => x.NumberOfCores).HasColumnName("number_of_cores");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
