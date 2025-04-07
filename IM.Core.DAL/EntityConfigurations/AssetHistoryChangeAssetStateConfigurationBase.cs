using InfraManager.DAL.Asset.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetHistoryChangeAssetStateConfigurationBase : IEntityTypeConfiguration<AssetHistoryChangeAssetState>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string AssetHistoryForeignKey { get; }
    protected abstract string LifeCycleStateForeignKey { get; }
    public void Configure(EntityTypeBuilder<AssetHistoryChangeAssetState> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.ReasonNumber).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.LifeCycleStateName).IsRequired(true).HasMaxLength(250);

        builder.HasOne(x => x.AssetHistory)
            .WithOne()
            .HasForeignKey<AssetHistoryChangeAssetState>(x => x.ID)
            .HasConstraintName(AssetHistoryForeignKey);

        builder.HasOne(x => x.LifeCycleState)
            .WithMany()
            .HasForeignKey(x => x.LifeCycleStateID)
            .HasConstraintName(LifeCycleStateForeignKey);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<AssetHistoryChangeAssetState> builder);
}
