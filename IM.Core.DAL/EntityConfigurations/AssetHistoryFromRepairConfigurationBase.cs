using InfraManager.DAL.Asset.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetHistoryFromRepairConfigurationBase : IEntityTypeConfiguration<AssetHistoryFromRepair>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string AssetHistoryForeignKey { get; }
    public void Configure(EntityTypeBuilder<AssetHistoryFromRepair> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.RepairType).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.Quality).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.Agreement).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.ReasonNumber).IsRequired(false).HasMaxLength(50);

        builder.HasOne(x => x.AssetHistory)
            .WithOne()
            .HasForeignKey<AssetHistoryFromRepair>(x => x.ID)
            .HasConstraintName(AssetHistoryForeignKey);

        ConfigureDatabase(builder);
    }
    protected abstract void ConfigureDatabase(EntityTypeBuilder<AssetHistoryFromRepair> builder);

}
