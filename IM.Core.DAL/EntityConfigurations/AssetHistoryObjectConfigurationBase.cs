using InfraManager.DAL.Asset.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetHistoryObjectConfigurationBase : IEntityTypeConfiguration<AssetHistoryObject>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string AssetHistoryForeignKey { get; }
    protected abstract string ObjectIDIndexName { get; }

    public void Configure(EntityTypeBuilder<AssetHistoryObject> builder)
    {
        builder.HasKey(x => new { x.ID, x.ObjectID }).HasName(PrimaryKeyName);

        builder.Property(x => x.ObjectName).IsRequired(true).HasMaxLength(250);

        builder.HasOne(x => x.AssetHistory)
            .WithOne()
            .HasForeignKey<AssetHistoryObject>(x => x.ID)
            .HasConstraintName(AssetHistoryForeignKey);

        builder.HasIndex(x => x.ObjectID).HasDatabaseName(ObjectIDIndexName);

        ConfigureDatabase(builder);
    }
    protected abstract void ConfigureDatabase(EntityTypeBuilder<AssetHistoryObject> builder);
}
