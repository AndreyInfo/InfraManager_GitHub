using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CallUserFieldNameConfigurationBase : IEntityTypeConfiguration<CallUserFieldName>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<CallUserFieldName> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.Property(x => x.Name).HasMaxLength(250).IsRequired(true);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CallUserFieldName> builder);
}
