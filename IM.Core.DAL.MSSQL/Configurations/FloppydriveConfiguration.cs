using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class FloppydriveConfiguration : FloppydriveConfigurationBase
{
    protected override string PrimaryKeyName => "PK_FloppyDrives";

    protected override void ConfigureDatabase(EntityTypeBuilder<Floppydrive> builder)
    {
        builder.ToTable("FloppyDrives", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.Heads).HasColumnName("Heads");
        builder.Property(x => x.Cylinders).HasColumnName("Cylinders");
        builder.Property(x => x.Sectors).HasColumnName("Sectors");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
