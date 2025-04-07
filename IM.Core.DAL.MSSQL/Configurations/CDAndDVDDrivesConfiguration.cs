using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class CDAndDVDDrivesConfiguration : CDAndDVDDrivesConfigurationBase
{
    protected override string PrimaryKeyName => "PK_CDAndDVDDrives";

    protected override void ConfigureDatabase(EntityTypeBuilder<CDAndDVDDrives> builder)
    {
        builder.ToTable("CDAndDVDDrives", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.WriteSpeed).HasColumnName("WriteSpeed");
        builder.Property(x => x.ReadSpeed).HasColumnName("ReadSpeed");
        builder.Property(x => x.DriveCapabilities).HasColumnName("DriveCapabilities");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}