using InfraManager.DAL.Asset.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetHistoryConfigurationBase : IEntityTypeConfiguration<AssetHistory>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string OperationTypeIndexName { get; }

    public void Configure(EntityTypeBuilder<AssetHistory> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.UserFullName).IsRequired(true).HasMaxLength(250);

        builder.HasIndex(x => x.OperationType).HasDatabaseName(OperationTypeIndexName);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<AssetHistory> builder);
}
