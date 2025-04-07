using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class AssetHistoryObjectConfiguration : AssetHistoryObjectConfigurationBase
{
    protected override string PrimaryKeyName => "pk_asset_history_object";
    protected override string AssetHistoryForeignKey => "fk_asset_history_object_asset_history";
    protected override string ObjectIDIndexName => "ix_asset_history_object_object_id";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryObject> builder)
    {
        builder.ToTable("asset_history_object", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(x => x.ObjectID).HasColumnName("object_id");
        builder.Property(x => x.ObjectName).HasColumnName("object_name");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
    }
}
