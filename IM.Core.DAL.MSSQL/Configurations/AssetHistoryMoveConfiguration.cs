using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class AssetHistoryMoveConfiguration : AssetHistoryMoveConfigurationBase
{
    protected override string PrimaryKeyName => "PK_AssetHistoryMove";
    protected override string AssetHistoryForeignKey => "FK_AssetHistoryMove_AssetHistory";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryMove> builder)
    {
        builder.ToTable("AssetHistoryMove", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.NewLocationClassID).HasColumnName("NewLocationClassID");
        builder.Property(x => x.NewLocationID).HasColumnName("NewLocationID");
        builder.Property(x => x.NewLocationName).HasColumnName("NewLocationName");
        builder.Property(x => x.ReasonNumber).HasColumnName("ReasonNumber");
        builder.Property(x => x.UtilizerClassID).HasColumnName("UtilizerClassID");
        builder.Property(x => x.UtilizerID).HasColumnName("UtilizerID");
        builder.Property(x => x.UtilizerName).HasColumnName("UtilizerName");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
    }
}
