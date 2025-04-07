using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class AssetHistoryChangeAssetStateConfiguration : AssetHistoryChangeAssetStateConfigurationBase
{
    protected override string PrimaryKeyName => "pk_asset_history_change_asset_state";
    protected override string AssetHistoryForeignKey => "fk_asset_history_change_asset_state_asset_history";
    protected override string LifeCycleStateForeignKey => "fk_asset_history_change_asset_state_life_cycle_state";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryChangeAssetState> builder)
    {
        builder.ToTable("asset_history_change_asset_state", Options.Scheme);
        
        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.ReasonNumber).HasColumnName("reason_number");
        builder.Property(x => x.LifeCycleStateID).HasColumnName("life_cycle_state_id");
        builder.Property(x => x.LifeCycleStateName).HasColumnName("life_cycle_state_name");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
    }
}
