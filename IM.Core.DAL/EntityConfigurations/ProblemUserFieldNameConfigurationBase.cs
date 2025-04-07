using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ProblemUserFieldNameConfigurationBase : IEntityTypeConfiguration<ProblemUserFieldName>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<ProblemUserFieldName> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.Property(c => c.Name).HasMaxLength(250).IsRequired(true);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ProblemUserFieldName> builder);
}
