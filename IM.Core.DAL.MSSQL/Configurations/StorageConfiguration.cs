using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class StorageConfiguration : StorageConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Storage";

    protected override void ConfigureDatabase(EntityTypeBuilder<Storage> builder)
    {
        builder.ToTable("Storage", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.FormattedCapacity).HasColumnName("FormattedCapacity");
        builder.Property(x => x.RecordingSurfaces).HasColumnName("RecordingSurfaces");
        builder.Property(x => x.InterfaceType).HasColumnName("InterfaceType");
        builder.Property(x => x.StorageClassID).HasColumnName("StorageClassID");
        builder.Property(x => x.StorageID).HasColumnName("StorageID");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
