using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class AssetHistoryFromRepairConfiguration : AssetHistoryFromRepairConfigurationBase
{
    protected override string PrimaryKeyName => "PK_AssetHistoryFromRepair";

    protected override string AssetHistoryForeignKey => "FK_AssetHistoryFromRepair_AssetHistory";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryFromRepair> builder)
    {
        builder.ToTable("AssetHistoryFromRepair", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.RepairType).HasColumnName("RepairType");
        builder.Property(x => x.Cost).HasColumnName("Cost");
        builder.Property(x => x.Quality).HasColumnName("Quality");
        builder.Property(x => x.Agreement).HasColumnName("Agreement");
        builder.Property(x => x.ReasonNumber).HasColumnName("ReasonNumber");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
    }
}
