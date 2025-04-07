using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class StorageConfigurationBase : IEntityTypeConfiguration<Storage>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Storage> builder);

    public void Configure(EntityTypeBuilder<Storage> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.FormattedCapacity).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.RecordingSurfaces).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.InterfaceType).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.StorageClassID).IsRequired(false);
        builder.Property(x => x.StorageID).IsRequired(false);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}