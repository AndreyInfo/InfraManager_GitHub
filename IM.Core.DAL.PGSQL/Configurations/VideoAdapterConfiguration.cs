using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class VideoAdapterConfiguration : VideoAdapterConfigurationBase
{
    protected override string PrimaryKeyName => "pk_video_adapter";

    protected override void ConfigureDatabase(EntityTypeBuilder<VideoAdapter> builder)
    {
        builder.ToTable("video_adapter", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.MemorySize).HasColumnName("memory_size");
        builder.Property(x => x.VideoModeDescription).HasColumnName("video_mode_description");
        builder.Property(x => x.ChipType).HasColumnName("chip_type");
        builder.Property(x => x.DacType).HasColumnName("dac_type");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
