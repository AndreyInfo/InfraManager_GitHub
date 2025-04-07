using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ProblemUserFieldNameConfiguration : ProblemUserFieldNameConfigurationBase
{
    protected override string KeyName => "PK_ProblemAdditionalField";

    protected override void ConfigureDataBase(EntityTypeBuilder<ProblemUserFieldName> builder)
    {
        builder.ToTable("ProblemAdditionalField", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
    }
}
