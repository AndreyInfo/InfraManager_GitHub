using InfraManager.DAL.Asset.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetHistoryToRepairConfigurationBase : IEntityTypeConfiguration<AssetHistoryToRepair>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string AssetHistoryForeignKey { get; }
    public void Configure(EntityTypeBuilder<AssetHistoryToRepair> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.ServiceCenterName).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.ServiceContractNumber).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.Problems).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.ReasonNumber).IsRequired(false).HasMaxLength(50);

        builder.HasOne(x => x.AssetHistory)
            .WithOne()
            .HasForeignKey<AssetHistoryToRepair>(x => x.ID)
            .HasConstraintName(AssetHistoryForeignKey);

        ConfigureDatabase(builder);
    }
    protected abstract void ConfigureDatabase(EntityTypeBuilder<AssetHistoryToRepair> builder);
}
