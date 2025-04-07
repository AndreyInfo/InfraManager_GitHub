using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class AssetUserFieldNameConfiguration : AssetUserFieldNameConfigurationBase
{
    protected override string KeyName => "PK_FieldName";

    protected override void ConfigureDataBase(EntityTypeBuilder<AssetUserFieldName> builder)
    {
        builder.ToTable("AssetFieldName", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("FieldID");
        builder.Property(x => x.Name).HasColumnName("FieldName");
    }
}
