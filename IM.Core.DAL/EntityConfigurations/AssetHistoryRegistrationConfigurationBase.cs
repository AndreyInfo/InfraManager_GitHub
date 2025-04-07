using InfraManager.DAL.Asset.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetHistoryRegistrationConfigurationBase : IEntityTypeConfiguration<AssetHistoryRegistration>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string AssetHistoryForeignKey { get; }
    public void Configure(EntityTypeBuilder<AssetHistoryRegistration> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.NewLocationName).IsRequired(true).HasMaxLength(500);
        builder.Property(x => x.OwnerName).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.UserFullName).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.Founding).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.NewStorageLocationName).IsRequired(false).HasMaxLength(250);

        builder.HasOne(x => x.AssetHistory)
            .WithOne()
            .HasForeignKey<AssetHistoryRegistration>(x => x.ID)
            .HasConstraintName(AssetHistoryForeignKey);

        ConfigureDatabase(builder);
    }
    protected abstract void ConfigureDatabase(EntityTypeBuilder<AssetHistoryRegistration> builder);
}
