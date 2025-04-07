using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class VideoAdapterConfigurationBase : IEntityTypeConfiguration<VideoAdapter>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<VideoAdapter> builder);

    public void Configure(EntityTypeBuilder<VideoAdapter> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.MemorySize).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.VideoModeDescription).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ChipType).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.DacType).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}
