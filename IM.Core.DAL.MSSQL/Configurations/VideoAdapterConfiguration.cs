using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class VideoAdapterConfiguration : VideoAdapterConfigurationBase
{
    protected override string PrimaryKeyName => "PK_VideoAdapter";

    protected override void ConfigureDatabase(EntityTypeBuilder<VideoAdapter> builder)
    {
        builder.ToTable("VideoAdapter", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.MemorySize).HasColumnName("MemorySize");
        builder.Property(x => x.VideoModeDescription).HasColumnName("VideoModeDescription");
        builder.Property(x => x.ChipType).HasColumnName("ChipType");
        builder.Property(x => x.DacType).HasColumnName("DACType");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
