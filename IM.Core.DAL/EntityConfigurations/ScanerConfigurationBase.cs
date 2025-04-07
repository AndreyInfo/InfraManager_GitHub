using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ScanerConfigurationBase : IEntityTypeConfiguration<Scaner>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Scaner> builder);

    public void Configure(EntityTypeBuilder<Scaner> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.ConnectorType).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}