using InfraManager.DAL.Asset.History;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class AssetHistoryRegistrationConfiguration : AssetHistoryRegistrationConfigurationBase
{
    protected override string PrimaryKeyName => "PK_AssetHistoryRegistration";
    protected override string AssetHistoryForeignKey => "FK_AssetHistoryRegistration_AssetHistory";

    protected override void ConfigureDatabase(EntityTypeBuilder<AssetHistoryRegistration> builder)
    {
        builder.ToTable("AssetHistoryRegistration", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.NewLocationClassID).HasColumnName("NewLocationClassID");
        builder.Property(x => x.NewLocationID).HasColumnName("NewLocationID");
        builder.Property(x => x.NewLocationName).HasColumnName("NewLocationName");
        builder.Property(x => x.OwnerClassID).HasColumnName("OwnerClassID");
        builder.Property(x => x.OwnerID).HasColumnName("OwnerID");
        builder.Property(x => x.OwnerName).HasColumnName("OwnerName");
        builder.Property(x => x.UserID).HasColumnName("UserID");
        builder.Property(x => x.UserFullName).HasColumnName("UserFullName");
        builder.Property(x => x.Founding).HasColumnName("Founding");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(x => x.NewStorageLocationID).HasColumnName("NewStorageLocationID");
        builder.Property(x => x.NewStorageLocationName).HasColumnName("NewStorageLocationName");
    }
}
