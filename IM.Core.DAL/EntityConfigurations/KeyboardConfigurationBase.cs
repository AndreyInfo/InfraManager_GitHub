using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class KeyboardConfigurationBase : IEntityTypeConfiguration<Keyboard>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Keyboard> builder);

    public void Configure(EntityTypeBuilder<Keyboard> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.DelayPeriod).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.NumberKeys).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Layout).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ConnectorType).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}