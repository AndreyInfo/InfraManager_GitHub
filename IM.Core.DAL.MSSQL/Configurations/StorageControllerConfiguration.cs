using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class StorageControllerConfiguration : StorageControllerConfigurationBase
{
    protected override string PrimaryKeyName => "PK_StorageController";

    protected override void ConfigureDatabase(EntityTypeBuilder<StorageController> builder)
    {
        builder.ToTable("StorageController", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.WWn).HasColumnName("WWN");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
