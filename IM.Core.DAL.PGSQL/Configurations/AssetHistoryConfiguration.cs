using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class AssetHistoryConfiguration : AssetHistoryConfigurationBase
{
    protected override string PrimaryKeyName => "pk_asset_history";
    protected override string OperationTypeIndexName => "ix_asset_history_operation_type";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistory> builder)
    {
        builder.ToTable("asset_history", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.UtcDate).HasColumnName("utc_date");
        builder.Property(x => x.UserID).HasColumnName("user_id");
        builder.Property(x => x.UserFullName).HasColumnName("user_full_name");
        builder.Property(x => x.OperationType).HasColumnName("operation_type");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
    }
}
