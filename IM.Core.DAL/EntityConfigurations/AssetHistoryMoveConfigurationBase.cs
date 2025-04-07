using InfraManager.DAL.Asset.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetHistoryMoveConfigurationBase : IEntityTypeConfiguration<AssetHistoryMove>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string AssetHistoryForeignKey { get; }
    public void Configure(EntityTypeBuilder<AssetHistoryMove> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.NewLocationName).IsRequired(true).HasMaxLength(500);
        builder.Property(x => x.ReasonNumber).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.UtilizerName).IsRequired(false).HasMaxLength(250);

        builder.HasOne(x => x.AssetHistory)
            .WithOne()
            .HasForeignKey<AssetHistoryMove>(x => x.ID)
            .HasConstraintName(AssetHistoryForeignKey);

        ConfigureDatabase(builder);
    }
    protected abstract void ConfigureDatabase(EntityTypeBuilder<AssetHistoryMove> builder);
}
