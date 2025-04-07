using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class AssetHistoryConfiguration : AssetHistoryConfigurationBase
{
    protected override string PrimaryKeyName => "PK_AssetHistory";
    protected override string OperationTypeIndexName => "IX_AssetHistory_OperationType";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistory> builder)
    {
        builder.ToTable("AssetHistory", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.UtcDate).HasColumnName("UtcDate");
        builder.Property(x => x.UserID).HasColumnName("UserID");
        builder.Property(x => x.UserFullName).HasColumnName("UserFullName");
        builder.Property(x => x.OperationType).HasColumnName("OperationType");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
    }
}
