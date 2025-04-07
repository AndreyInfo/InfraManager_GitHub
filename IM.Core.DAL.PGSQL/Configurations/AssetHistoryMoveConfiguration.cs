using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class AssetHistoryMoveConfiguration : AssetHistoryMoveConfigurationBase
{
    protected override string PrimaryKeyName => "pk_asset_history_move";
    protected override string AssetHistoryForeignKey => "fk_asset_history_move_asset_history";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryMove> builder)
    {
        builder.ToTable("asset_history_move", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.NewLocationClassID).HasColumnName("new_location_class_id");
        builder.Property(x => x.NewLocationID).HasColumnName("new_location_id");
        builder.Property(x => x.NewLocationName).HasColumnName("new_location_name");
        builder.Property(x => x.ReasonNumber).HasColumnName("reason_number");
        builder.Property(x => x.UtilizerClassID).HasColumnName("utilizer_class_id");
        builder.Property(x => x.UtilizerID).HasColumnName("utilizer_id");
        builder.Property(x => x.UtilizerName).HasColumnName("utilizer_name");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
    }
}
