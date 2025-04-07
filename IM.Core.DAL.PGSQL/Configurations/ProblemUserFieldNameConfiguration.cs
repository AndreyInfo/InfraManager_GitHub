using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal class ProblemUserFieldNameConfiguration : ProblemUserFieldNameConfigurationBase
{
    protected override string KeyName => "problem_additional_field_pkey";

    protected override void ConfigureDataBase(EntityTypeBuilder<ProblemUserFieldName> builder)
    {
        builder.ToTable("problem_additional_field", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
    }
}