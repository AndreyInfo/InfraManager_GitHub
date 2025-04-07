using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class AssetUserFieldNameConfiguration : AssetUserFieldNameConfigurationBase
{
    protected override string KeyName => "pk_field_name";

    protected override void ConfigureDataBase(EntityTypeBuilder<AssetUserFieldName> builder)
    {
        builder.ToTable("asset_field_name", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("field_id");
        builder.Property(e => e.Name).HasColumnName("field_name");
    }
}