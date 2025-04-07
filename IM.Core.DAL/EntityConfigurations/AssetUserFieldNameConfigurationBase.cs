using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AssetUserFieldNameConfigurationBase : IEntityTypeConfiguration<AssetUserFieldName>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<AssetUserFieldName> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.Property(c => c.Name).HasMaxLength(50).IsRequired(false);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<AssetUserFieldName> builder);
}
