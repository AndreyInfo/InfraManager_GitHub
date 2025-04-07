using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class AssetHistoryRegistrationConfiguration : AssetHistoryRegistrationConfigurationBase
{
    protected override string PrimaryKeyName => "pk_asset_history_registration";
    protected override string AssetHistoryForeignKey => "fk_asset_history_registration_asset_history";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryRegistration> builder)
    {
        builder.ToTable("asset_history_registration", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.NewLocationClassID).HasColumnName("new_location_class_id");
        builder.Property(x => x.NewLocationID).HasColumnName("new_location_id");
        builder.Property(x => x.NewLocationName).HasColumnName("new_location_name");
        builder.Property(x => x.OwnerClassID).HasColumnName("owner_class_id");
        builder.Property(x => x.OwnerID).HasColumnName("owner_id");
        builder.Property(x => x.OwnerName).HasColumnName("owner_name");
        builder.Property(x => x.UserID).HasColumnName("user_id");
        builder.Property(x => x.UserFullName).HasColumnName("user_full_name");
        builder.Property(x => x.Founding).HasColumnName("founding");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(x => x.NewStorageLocationID).HasColumnName("new_storage_location_id");
        builder.Property(x => x.NewStorageLocationName).HasColumnName("new_storage_location_name");
    }
}
