using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class AssetHistoryFromRepairConfiguration : AssetHistoryFromRepairConfigurationBase
{
    protected override string PrimaryKeyName => "pk_asset_history_from_repair";
    protected override string AssetHistoryForeignKey => "fk_asset_history_from_repair_asset_history";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryFromRepair> builder)
    {
        builder.ToTable("asset_history_from_repair", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.RepairType).HasColumnName("repair_type");
        builder.Property(x => x.Cost).HasColumnName("cost");
        builder.Property(x => x.Quality).HasColumnName("quality");
        builder.Property(x => x.Agreement).HasColumnName("agreement");
        builder.Property(x => x.ReasonNumber).HasColumnName("reason_number");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
    }
}
