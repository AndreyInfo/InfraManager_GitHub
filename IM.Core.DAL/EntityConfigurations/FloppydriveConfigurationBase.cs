using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FloppydriveConfigurationBase : IEntityTypeConfiguration<Floppydrive>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Floppydrive> builder);

    public void Configure(EntityTypeBuilder<Floppydrive> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Heads).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Cylinders).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Sectors).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}