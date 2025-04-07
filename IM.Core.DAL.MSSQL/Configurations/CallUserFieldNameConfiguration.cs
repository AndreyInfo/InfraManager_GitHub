using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CallUserFieldNameConfiguration : CallUserFieldNameConfigurationBase
{
    protected override string KeyName => "PK_CallAdditionalField";

    protected override void ConfigureDataBase(EntityTypeBuilder<CallUserFieldName> builder)
    {
        builder.ToTable("CallAdditionalField", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
    }
}
