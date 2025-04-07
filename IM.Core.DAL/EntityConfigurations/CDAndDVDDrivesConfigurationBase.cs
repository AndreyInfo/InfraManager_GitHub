using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CDAndDVDDrivesConfigurationBase : IEntityTypeConfiguration<CDAndDVDDrives>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<CDAndDVDDrives> builder);

    public void Configure(EntityTypeBuilder<CDAndDVDDrives> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.WriteSpeed).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ReadSpeed).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.DriveCapabilities).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}