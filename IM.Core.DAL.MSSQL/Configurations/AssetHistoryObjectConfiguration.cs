using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class AssetHistoryObjectConfiguration : AssetHistoryObjectConfigurationBase
{
    protected override string PrimaryKeyName => "PK_AssetHistoryObject";
    protected override string AssetHistoryForeignKey => "FK_AssetHistoryObject_AssetHistory";
    protected override string ObjectIDIndexName => "IX_AssetHistoryObject_ObjectID";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryObject> builder)
    {
        builder.ToTable("AssetHistoryObject", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
        builder.Property(x => x.ObjectName).HasColumnName("ObjectName");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
    }
}
