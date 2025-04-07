using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class AssetHistoryChangeAssetStateConfiguration : AssetHistoryChangeAssetStateConfigurationBase
{
    protected override string PrimaryKeyName => "PK_AssetHistoryChangeAssetState";
    protected override string AssetHistoryForeignKey => "FK_AssetHistoryChangeAssetState_AssetHistory";
    protected override string LifeCycleStateForeignKey => "FK_AssetHistoryChangeAssetState_LifeCycleState";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryChangeAssetState> builder)
    {
        builder.ToTable("AssetHistoryChangeAssetState", Options.Scheme);
        
        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.ReasonNumber).HasColumnName("ReasonNumber");
        builder.Property(x => x.LifeCycleStateID).HasColumnName("LifeCycleStateID");
        builder.Property(x => x.LifeCycleStateName).HasColumnName("LifeCycleStateName");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
    }
}
