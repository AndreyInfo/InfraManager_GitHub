using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class AssetHistoryToRepairConfiguration : AssetHistoryToRepairConfigurationBase
{
    protected override string PrimaryKeyName => "pk_asset_history_to_repair";
    protected override string AssetHistoryForeignKey => "fk_asset_history_to_repair_asset_history";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryToRepair> builder)
    {
        builder.ToTable("asset_history_to_repair", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.LocationClassID).HasColumnName("location_class_id");
        builder.Property(x => x.LocationID).HasColumnName("location_id");
        builder.Property(x => x.UtcDateAnticipated).HasColumnName("utc_date_anticipated");
        builder.Property(x => x.ServiceCenterID).HasColumnName("service_center_id");
        builder.Property(x => x.ServiceCenterName).HasColumnName("service_center_name");
        builder.Property(x => x.ServiceContractID).HasColumnName("service_contract_id");
        builder.Property(x => x.ServiceContractNumber).HasColumnName("service_contract_number");
        builder.Property(x => x.Problems).HasColumnName("problems");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(x => x.ReasonNumber).HasColumnName("reason_number");
    }
}
