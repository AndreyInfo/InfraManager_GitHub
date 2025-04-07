using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class StorageControllerConfigurationBase : IEntityTypeConfiguration<StorageController>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<StorageController> builder);

    public void Configure(EntityTypeBuilder<StorageController> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.WWn).IsRequired(true);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}