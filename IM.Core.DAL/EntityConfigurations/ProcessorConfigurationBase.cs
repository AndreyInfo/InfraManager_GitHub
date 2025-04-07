using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ProcessorConfigurationBase : IEntityTypeConfiguration<Processor>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Processor> builder);

    public void Configure(EntityTypeBuilder<Processor> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.MaxClockSpeed).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.CurrentClockSpeed).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.L2cacheSize).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.SocketDesignation).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Platform).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.NumberOfCores).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}