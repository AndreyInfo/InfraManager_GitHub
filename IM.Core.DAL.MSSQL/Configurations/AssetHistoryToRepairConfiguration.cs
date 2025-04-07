using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class AssetHistoryToRepairConfiguration : AssetHistoryToRepairConfigurationBase
{
    protected override string PrimaryKeyName => "PK_AssetHistoryToRepair";
    protected override string AssetHistoryForeignKey => "FK_AssetHistoryToRepair_AssetHistory";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryToRepair> builder)
    {
        builder.ToTable("AssetHistoryToRepair", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.LocationClassID).HasColumnName("LocationClassID");
        builder.Property(x => x.LocationID).HasColumnName("LocationID");
        builder.Property(x => x.UtcDateAnticipated).HasColumnName("UtcDateAnticipated");
        builder.Property(x => x.ServiceCenterID).HasColumnName("ServiceCenterID");
        builder.Property(x => x.ServiceCenterName).HasColumnName("ServiceCenterName");
        builder.Property(x => x.ServiceContractID).HasColumnName("ServiceContractID");
        builder.Property(x => x.ServiceContractNumber).HasColumnName("ServiceContractNumber");
        builder.Property(x => x.Problems).HasColumnName("Problems");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(x => x.ReasonNumber).HasColumnName("ReasonNumber");
    }
}
