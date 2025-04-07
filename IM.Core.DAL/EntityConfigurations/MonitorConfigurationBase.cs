using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MonitorConfigurationBase : IEntityTypeConfiguration<Monitor>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Monitor> builder);

    public void Configure(EntityTypeBuilder<Monitor> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Resolution).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.FontResolution).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Diagonal).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}